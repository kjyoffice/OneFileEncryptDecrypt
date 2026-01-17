using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XWork
{
    public class DecryptWork
    {
        private static bool IsAllowSourceAndFinalFilePath(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms, XModel.CryptoWorkOrder cwo, string decryptOriginalFIlePath)
        {
            var result = false;

            // 원본파일 역시 존재하면 안된다
            if (File.Exists(decryptOriginalFIlePath) == false)
            {
                // 파일 확장자 체크랑 ZIP 파일인지 체크한다
                if ((Path.GetExtension(cwo.SourceFilePath).ToUpper() == ".OFEDX") && (FileWork.IsZIPFileMagicByte(cwo.SourceFilePath) == true))
                {
                    // OK
                    result = true;
                }
                else
                {
                    // 복호화 파일이 올바르지 않습니다.
                    cwms.Error.MessageNow(asx.WorkMessage.DecryptFileWrong);
                }
            }
            else
            {
                // 같은 이름의 암호화된 파일이 있습니다.
                // 진행이 중단되었습니다.
                cwms.Error.MessageNow(asx.WorkMessage.AlreadyExistDecryptFile);
            }

            return result;
        }

        private static void ZIPExtract(XAppSettings.AppSettingsX asx, XModel.CryptoXFilePath cfn, XModel.CryptoWorkOrder cwo, XModel.ProgressViewer pv)
        {
            // 우선 파일 압축을 해제한다
            FileWork.ZIPExtract(cwo.SourceFilePath, cfn.WorkDirectoryPath, asx.WorkMessage.ZIPExtractFile, pv);
        }

        private static XModel.EncryptDataHMAC GetEncryptData(XAppSettings.AppSettingsX asx, XModel.CryptoXFilePath cfn, XCrypto.CryptoKeySet cks, XModel.ProgressViewer pv)
        {
            // 복호화 IV 읽기
            var cryptoIV = File.ReadAllBytes(cfn.CryptoIVFilePath);
            // 암호화 된 파일 읽기
            var encryptData = FileWork.GetFileByte(cfn.EncryptDataFilePath, asx.WorkMessage.ReadFile, pv);
            // 암호화 된 파일 HMAC 만들기
            var encryptHMAC = XCrypto.HashWork.CreateSHA512HMAC(encryptData, cks.GetCryptoHMACKey, asx.WorkMessage.EncryptHMAC, pv);
            var result = new XModel.EncryptDataHMAC(cryptoIV, encryptData, encryptHMAC);

            return result;
        }

        private static bool IsMatchEncryptHMAC(XModel.CryptoXFilePath cfn, XModel.EncryptDataHMAC edh)
        {
            // 암호화된 파일 읽은 후 만들어진 HMAC
            var encryptHMAC = XCrypto.HashWork.ConvertHashText(edh.EncryptHMAC);
            // 암호화 할 때 만들어둔 HMAC
            var encryptHMACChecker = XCrypto.HashWork.ConvertHashText(File.ReadAllBytes(cfn.EncryptHMACFilePath));
            var result = (encryptHMAC == encryptHMACChecker);

            return result;
        }

        private static XModel.OriginalDataHMAC GetOriginalData(XAppSettings.AppSettingsX asx, XCrypto.CryptoKeySet cks, XModel.ProgressViewer pv, XModel.EncryptDataHMAC edh)
        {
            // 파일 복호화
            var originalData = XCrypto.AES256CBC.DecryptNow(cks.GetCryptoKey, edh.CryptoIV, edh.EncryptData, asx.WorkMessage.DecryptFile, pv);
            // 복호화 된 파일 HMAC 만들기
            var originalHMAC = XCrypto.HashWork.CreateSHA512HMAC(originalData, cks.GetOriginalHMACKey, asx.WorkMessage.DecryptHMAC, pv);
            var result = new XModel.OriginalDataHMAC(originalData, originalHMAC);

            return result;
        }

        private static bool IsMatchOriginalHMAC(XModel.CryptoXFilePath cfn, XModel.OriginalDataHMAC odh)
        {
            // 복호화 후 HMAC
            var originalHMAC = XCrypto.HashWork.ConvertHashText(odh.OriginalHMAC);
            // 암호화 할 때 만들어둔 원본 HMAC
            var originalHMACChecker = XCrypto.HashWork.ConvertHashText(File.ReadAllBytes(cfn.OriginalHMACFilePath));
            var result = (originalHMAC == originalHMACChecker);

            return result;
        }

        private static void ErrorMessage(XConsole.ConsoleWriteMessageSet cwms, XModel.CryptoXFilePath cfn, string message)
        {
            // 작업이 중단되었으니 압축풀어진 파일들 삭제
            cfn.DeleteAllFile(string.Empty);

            cwms.EmptyLine();
            cwms.Error.MessageNow(message);
        }

        private static void SuccessMessage(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms, XModel.CryptoWorkOrder cwo, XModel.CryptoXFilePath cfn)
        {
            // 복호화 성공했으니 암호화 된 파일을 포함 작업파일 삭제
            cfn.DeleteAllFile(cwo.SourceFilePath);

            cwms.EmptyLine();
            // 파일을 복호화 했습니다.
            cwms.Success.MessageNow(asx.WorkMessage.DecryptFileDone);
        }

        // ---------------------------------------------------------------------------------------------

        public static void ExecuteNow(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms, XModel.CryptoWorkOrder cwo)
        {
            // 복호화 후 원본파일 경로
            var decryptOriginalFIlePath = cwo.CreateDecryptOriginalFIlePath();

            // 소스파일과 작업파일 체크
            if (DecryptWork.IsAllowSourceAndFinalFilePath(asx, cwms, cwo, decryptOriginalFIlePath) == true)
            {
                // 저장 할 파일들 경로생성
                var cfn = asx.Crypto.CreateCryptoWorkPath();
                var pv = new XModel.ProgressViewer();

                // 우선 파일 압축을 해제한다
                DecryptWork.ZIPExtract(asx, cfn, cwo, pv);

                // 복호화 필수 파일들이 있는지 체크
                // 아무래도 zip 파일 경로 아무거나 넣으면 일단 압축을 풀거기 때문에 필수 파일이 모두 있는지 체크함
                if (cfn.IsAllExistDecryptFile == true)
                {
                    // 키 셋트 생성
                    var cks = new XCrypto.CryptoKeySet(asx, cwo);
                    // 암호화 된 파일 읽기
                    var edh = DecryptWork.GetEncryptData(asx, cfn, cks, pv);

                    // 암호화 된 파일 HMAC 비교
                    if (DecryptWork.IsMatchEncryptHMAC(cfn, edh) == true)
                    {
                        // 파일 복호화
                        var odh = DecryptWork.GetOriginalData(asx, cks, pv, edh);

                        if (DecryptWork.IsMatchOriginalHMAC(cfn, odh) == true)
                        {
                            // 원본파일 저장
                            FileWork.WriteFileByte(odh.OriginalData, decryptOriginalFIlePath, asx.WorkMessage.SaveDecryptFile, pv);

                            // Success
                            DecryptWork.SuccessMessage(asx, cwms, cwo, cfn);
                        }
                        else
                        {
                            // 복호화 파일 HMAC가 다릅니다.
                            DecryptWork.ErrorMessage(cwms, cfn, asx.WorkMessage.DifferentDecryptHMAC);
                        }
                    }
                    else
                    {
                        // 암호화 파일 HMAC가 다릅니다.
                        DecryptWork.ErrorMessage(cwms, cfn, asx.WorkMessage.DifferentEncryptHMAC);
                    }
                }
                else
                {
                    // 존재하지 않는 복호화 필수 파일이 있습니다.
                    DecryptWork.ErrorMessage(cwms, cfn, asx.WorkMessage.NotExistDecryptRequireFile);
                }
            }
        }
    }
}

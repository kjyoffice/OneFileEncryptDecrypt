using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XWork
{
    public class DecryptWork
    {
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


        // ---------------------------------------------------------------------------------------------



        public static void ExecuteNow(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms, XModel.CryptoWorkOrder cwo)
        {
            // 복호화 후 원본파일 경로
            var decryptOriginalFIlePath = cwo.CreateDecryptOriginalFIlePath;

            // 원본파일 역시 존재하면 안된다
            if (File.Exists(decryptOriginalFIlePath) == false)
            {
                // 파일 확장자 체크랑 ZIP 파일인지 체크한다
                if ((Path.GetExtension(cwo.SourceFilePath).ToUpper() == ".OFEDX") && (FileWork.IsZIPFileMagicByte(cwo.SourceFilePath) == true))
                {
                    // 저장 할 파일들 경로생성
                    var cfn = asx.Crypto.GetCryptoWorkPath;
                    var pv = new XModel.ProgressViewer();

                    // 우선 파일 압축을 해제한다
                    FileWork.ZIPExtract(cwo.SourceFilePath, cfn.WorkDirectoryPath, asx.WorkMessage.ZIPExtractFile, pv);

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

                                // 작업파일 삭제
                                cfn.DeleteAllFile(cwo.SourceFilePath);

                                cwms.EmptyLine();
                                // 파일을 복호화 했습니다.
                                cwms.Success.MessageNow(asx.WorkMessage.DecryptFileDone);
                            }
                            else
                            {
                                cwms.EmptyLine();
                                // 복호화 파일 HMAC가 다릅니다.
                                cwms.Error.MessageNow(asx.WorkMessage.DifferentDecryptHMAC);
                            }
                        }
                        else
                        {
                            cwms.EmptyLine();
                            // 암호화 파일 HMAC가 다릅니다.
                            cwms.Error.MessageNow(asx.WorkMessage.DifferentEncryptHMAC);
                        }
                    }
                    else
                    {
                        // 파일이 모두 없으니 있는 파일이라도 지운다
                        cfn.DeleteAllFile(string.Empty);

                        cwms.EmptyLine();
                        // 존재하지 않는 복호화 필수 파일이 있습니다.
                        cwms.Error.MessageNow(asx.WorkMessage.NotExistDecryptRequireFile);
                    }
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
        }
    }
}

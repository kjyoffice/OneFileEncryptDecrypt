using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XWork
{
    public class DecryptWork_AES256
    {
        private static void ErrorMessage(XConsole.ConsoleWriteMessageSet cwms, XModel.CryptoXFilePath cfn, string message)
        {
            // 작업이 중단되었으니 압축풀어진 파일들 삭제
            cfn.DeleteAllFile(string.Empty);

            cwms.EmptyLine();
            cwms.Error.MessageNow(message, true);
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

        private static XModel.OriginalDataHMAC GetOriginalData(XAppSettings.AppSettingsX asx, XCrypto.CryptoKeySet cks, XModel.ProgressViewer pv, XModel.EncryptDataHMAC edh, XModel.CryptoInfo ci)
        {
            var originalData = DecryptWork_AES256.GetOriginalData_Work(asx, cks, pv, edh, ci);
            // 복호화 된 파일 HMAC 만들기
            var originalHMAC = XCrypto.HashWork.CreateSHA512HMAC(originalData, cks.GetOriginalHMACKey, asx.WorkMessage.DecryptHMAC, pv);
            var result = new XModel.OriginalDataHMAC(originalData, originalHMAC);

            return result;
        }

        private static byte[] GetOriginalData_Work(XAppSettings.AppSettingsX asx, XCrypto.CryptoKeySet cks, XModel.ProgressViewer pv, XModel.EncryptDataHMAC edh, XModel.CryptoInfo ci)
        {
            byte[] result;

            if (ci.CryptoMode == XValue.ProcessValue.CryptoMode_AES256CBC)
            {
                result = XCrypto.AES256CBC.DecryptNow(edh.EncryptData, cks.GetCryptoKey, edh.CryptoIV, asx.WorkMessage.DecryptFile, pv);
            }
            else if (ci.CryptoMode == XValue.ProcessValue.CryptoMode_AES256GCM)
            {
                result = XCrypto.AES256GCM.DecryptNow(edh.EncryptData, cks.GetCryptoKey, cks.NonceSize, null, asx.WorkMessage.DecryptFile, pv);
            }
            else
            {
                result = Array.Empty<byte>();
            }

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

        private static void SuccessMessage(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms, XModel.CryptoWorkOrder cwo, XModel.CryptoXFilePath cfn)
        {
            // 복호화 성공했으니 암호화 된 파일을 포함 작업파일 삭제
            cfn.DeleteAllFile(cwo.SourceFilePath);

            cwms.EmptyLine();
            // 파일을 복호화 했습니다.
            cwms.Success.MessageNow(asx.WorkMessage.DecryptFileDone, true);
            cwms.EmptyLine();

            // Final Success!
            asx.FinalSuccessSign();
        }

        private static XCrypto.CryptoKeySet CreateCryptoKeySet(XModel.CryptoWorkOrder cwo, XModel.CryptoXFilePath cfn)
        {
            // Salt 정보 받아오기
            var salt = File.ReadAllBytes(cfn.CryptoSaltFilePath);
            // 키 셋트 생성
            var result = new XCrypto.CryptoKeySet(cwo, salt);

            return result;
        }

        // --------------------------------------------------------

        public static void ExecuteNow(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms, XModel.CryptoWorkOrder cwo, XModel.CryptoXFilePath cfn, XModel.ProgressViewer pv, string decryptOriginalFIlePath, XModel.CryptoInfo ci)
        {
            // 키 셋트 생성
            var cks = DecryptWork_AES256.CreateCryptoKeySet(cwo, cfn);
            // 암호화 된 파일 읽기
            var edh = DecryptWork_AES256.GetEncryptData(asx, cfn, cks, pv);

            // 암호화 된 파일 HMAC 비교
            if (DecryptWork_AES256.IsMatchEncryptHMAC(cfn, edh) == true)
            {
                // 파일 복호화
                var odh = DecryptWork_AES256.GetOriginalData(asx, cks, pv, edh, ci);

                if (DecryptWork_AES256.IsMatchOriginalHMAC(cfn, odh) == true)
                {
                    // 원본파일 저장
                    FileWork.WriteFileByte(odh.OriginalData, decryptOriginalFIlePath, asx.WorkMessage.SaveDecryptFile, pv);

                    // Success
                    DecryptWork_AES256.SuccessMessage(asx, cwms, cwo, cfn);
                }
                else
                {
                    // 복호화 파일 HMAC가 다릅니다.
                    DecryptWork_AES256.ErrorMessage(cwms, cfn, asx.WorkMessage.DifferentDecryptHMAC);
                }
            }
            else
            {
                // 암호화 파일 HMAC가 다릅니다.
                DecryptWork_AES256.ErrorMessage(cwms, cfn, asx.WorkMessage.DifferentEncryptHMAC);
            }
        }
    }
}

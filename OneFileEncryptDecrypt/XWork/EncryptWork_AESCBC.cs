using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XWork
{
    public class EncryptWork_AESCBC
    {
        private static XModel.OriginalDataHMAC GetOriginalData(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms, XModel.CryptoWorkOrder cwo, XCrypto.CryptoKeySet cks, XModel.ProgressViewer pv)
        {
            // 원본파일 읽기
            var originalData = FileWork.GetFileByte(cwo.SourceFilePath, asx.WorkMessage.ReadFile, pv);
            // 원본파일 HMAC
            var hmac = XCrypto.HashWork.CreateSHA512HMAC(originalData, cks.GetOriginalHMACKey, asx.WorkMessage.OriginalHMAC, pv);
            var result = new XModel.OriginalDataHMAC(originalData, hmac);

            return result;
        }

        private static XModel.EncryptDataHMAC GetEncryptData(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms, XCrypto.CryptoKeySet cks, XModel.ProgressViewer pv, XModel.OriginalDataHMAC odh)
        {
            var cryptoIV = cks.GetCryptoIV;
            // 파일 암호화
            var encryptData = XCrypto.AESCBC.EncryptNow(cks.GetCryptoKey, cryptoIV, odh.OriginalData, asx.WorkMessage.EncryptFile, pv);
            // 암호화 된 데이터 HMAC
            var hmac = XCrypto.HashWork.CreateSHA512HMAC(encryptData, cks.GetCryptoHMACKey, asx.WorkMessage.EncryptHMAC, pv);
            var result = new XModel.EncryptDataHMAC(cryptoIV, encryptData, hmac);

            return result;
        }

        private static void SaveOriginalData(XModel.CryptoXFilePath cfn, XModel.OriginalDataHMAC odh)
        {
            // 원본파일 HMAC 저장
            File.WriteAllBytes(cfn.OriginalHMACFilePath, odh.OriginalHMAC);
        }

        private static void SaveEncryptData(XAppSettings.AppSettingsX asx, XModel.CryptoXFilePath cfn, XModel.EncryptDataHMAC edh, XModel.ProgressViewer pv)
        {
            // 암호화 IV 저장
            File.WriteAllBytes(cfn.CryptoIVFilePath, edh.CryptoIV);
            // 암호화 된 데이터 HMAC 저장
            File.WriteAllBytes(cfn.EncryptHMACFilePath, edh.EncryptHMAC);
            // 암호화 된 데이터 저장
            FileWork.WriteFileByte(edh.EncryptData, cfn.EncryptDataFilePath, asx.WorkMessage.SaveEncryptFile, pv);
        }

        private static void CreateAndSaveCryptoInfo(XModel.CryptoXFilePath cfn)
        {
            // 암호화 정보 생성
            var infoText = JsonWork.ToJsonText(new XModel.CryptoInfo(XValue.ProcessValue.CryptoMode_AESCBC, 1));

            // 암호화 정보 저장
            File.WriteAllText(cfn.CryptoInfoFilePath, infoText, Encoding.UTF8);
        }

        private static void ZIPCompression(XAppSettings.AppSettingsX asx, XModel.CryptoXFilePath cfn, string encryptZIPFilePath, XModel.ProgressViewer pv)
        {
            // 파일들 압축
            FileWork.ZIPCompression(cfn.WorkDirectoryPath, encryptZIPFilePath, asx.WorkMessage.ZIPCompressionFile, pv);
        }

        private static void SuccessMessage(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms, XModel.CryptoWorkOrder cwo, XModel.CryptoXFilePath cfn)
        {
            // 작업파일 삭제
            cfn.DeleteAllFile(cwo.SourceFilePath);

            cwms.EmptyLine();
            // 암호화 비밀번호는 잊으면 안됩니다.
            // 잊지 않도록 기억해주세요!
            cwms.Warning.MessageNow(asx.WorkMessage.EncryptPasswordMemoryNotify, true);
            // 파일을 암호화 했습니다.
            cwms.Success.MessageNow(asx.WorkMessage.EncryptFileDone);
        }

        // ----------------------------------------------------------------------------------------------------------------------------

        public static void ExecuteNow(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms, XModel.CryptoWorkOrder cwo, XModel.CryptoXFilePath cfn, string encryptZIPFilePath)
        {
            var pv = new XModel.ProgressViewer();
            // 키 셋트 생성
            var cks = new XCrypto.CryptoKeySet(asx, cwo);
            // 원본파일 읽고, HMAC 만들기
            var odh = EncryptWork_AESCBC.GetOriginalData(asx, cwms, cwo, cks, pv);
            // 암호화 하고 HMAC 만들기
            var edh = EncryptWork_AESCBC.GetEncryptData(asx, cwms, cks, pv, odh);

            // 원본
            EncryptWork_AESCBC.SaveOriginalData(cfn, odh);
            // 암호화
            EncryptWork_AESCBC.SaveEncryptData(asx, cfn, edh, pv);
            // 암호화 정보
            EncryptWork_AESCBC.CreateAndSaveCryptoInfo(cfn);

            // 파일들 압축
            EncryptWork_AESCBC.ZIPCompression(asx, cfn, encryptZIPFilePath, pv);

            // Success
            EncryptWork_AESCBC.SuccessMessage(asx, cwms, cwo, cfn);
        }
    }
}

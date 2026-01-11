using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;

namespace OneFileEncryptDecrypt.XWork
{
    public class EncryptWork
    {
        public static void ExecuteNow(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms, XModel.CryptoWorkOrder cwo)
        {
            // 키 생성
            var cryptoKey = XCrypto.AES256Process.CreateKey(cwo.CryptoPassword, asx.Crypto.GetSalt);
            // IV 생성
            var cryptoIV = XCrypto.AES256Process.CreateIV();
            var pv = new XModel.ProgressViewer();
            // 파일 읽기
            var sourceBT = FileWork.GetFileByte(cwo.SourceFilePath, asx.WorkMessage.ReadFile, pv);
            // 원본파일 해쉬 만들기
            var originalChecksum = XCrypto.HashWork.CreateSHA512(sourceBT, asx.WorkMessage.OriginalChecksum, pv);
            // 파일 암호화
            var encryptData = XCrypto.AES256X.EncryptNow(cryptoKey, cryptoIV, sourceBT, asx.WorkMessage.EncryptFile, pv);
            // 암호화 된 파일 해쉬 만들기
            var encryptDataChecksum = XCrypto.HashWork.CreateSHA512(encryptData, asx.WorkMessage.EncryptChecksum, pv);
            // 저장 할 파일들 경로생성
            var cfn = asx.Crypto.GetCryptoWorkPath;

            // 원본파일 해쉬 저장
            File.WriteAllBytes(cfn.OriginalChecksumFilePath, originalChecksum);
            // 암호화 된 파일 해쉬 저장
            File.WriteAllBytes(cfn.EncryptDataChecksumFilePath, encryptDataChecksum);
            // 암호화 IV 저장
            File.WriteAllBytes(cfn.CryptoIVFilePath, cryptoIV);
            // 암호화 된 파일 저장
            FileWork.WriteFileByte(encryptData, cfn.EncryptDataFilePath, asx.WorkMessage.SaveEncryptFile, pv);
            // 파일들 압축
            FileWork.ZIPCompression(cfn.WorkDirPath, cwo.CreateEncryptZIPFilePath, asx.WorkMessage.SaveFinalFile, pv);
            // 원본파일 삭제
            File.Delete(cwo.SourceFilePath);

            // 파일을 암호화 했습니다.
            cwms.Success.MessageNow(asx.WorkMessage.EncryptFileDone);
        }
    }
}

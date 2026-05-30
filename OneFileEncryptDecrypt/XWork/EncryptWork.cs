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
            // 압축할 파일 경로
            var encryptZIPFilePath = cwo.CreateEncryptZIPFilePath();

            cwms.Success.MessageNow(XValue.ProcessValue.ApplicationPublicTitle);
            cwms.Normal.MessageNow(" - ");
            cwms.Warning.MessageNow("Encrypt", true);
            cwms.EmptyLine();
            cwms.Warning.MessageNow("[Source File] ");
            cwms.Success.MessageNow(cwo.SourceFilePath, true);
            cwms.Warning.MessageNow("[Target File] ");
            cwms.Success.MessageNow(encryptZIPFilePath, true);
            cwms.Warning.MessageNow("[Mode] ");
            cwms.Success.MessageNow(XValue.ProcessValue.CryptoModeDisplay[cwo.CryptoMode], true);
            cwms.EmptyLine();

            // TODO : 테스트때 필요시 활성화 
            // 이 부분 활성화 하면 실제 암호화때 원본파일 삭제하는거 비활성화
            // - 코드 찾기용 : cfn.DeleteAllFile(cwo.SourceFilePath);
            /*
            if (File.Exists(encryptZIPFilePath) == true)
            {
                File.Delete(encryptZIPFilePath);
            }
            */

            // 압축할 파일이 존재하지 않아야 한다!
            if (File.Exists(encryptZIPFilePath) == false)
            {
                // 저장 할 파일들 경로생성
                var cfn = asx.CreateCryptoWorkPath();

                if (cfn.IsEmptyDirectory == true)
                {
                    // 암호화 방법 체크
                    if ((cwo.CryptoMode == XValue.ProcessValue.CryptoMode_AES256CBC) || (cwo.CryptoMode == XValue.ProcessValue.CryptoMode_AES256GCM))
                    {
                        // AES256 CBC / GCM
                        EncryptWork_AES256.ExecuteNow(asx, cwms, cwo, cfn, encryptZIPFilePath);
                    }
                    else
                    {
                        // 지정되지 않은 암호화 작업입니다.
                        cwms.Error.MessageNow(asx.WorkMessage.UndefinedEncryptWork, true);
                    }
                }
                else
                {
                    // 암호화 작업 디렉토리가 비어있지 않습니다.
                    cwms.Error.MessageNow(asx.WorkMessage.NotEmptyEncryptDirectory, true);
                }
            }
            else
            {
                // 같은 이름의 암호화된 파일이 있습니다.
                cwms.Error.MessageNow(asx.WorkMessage.AlreadyExistEncryptFile, true);
            }
        }
    }
}

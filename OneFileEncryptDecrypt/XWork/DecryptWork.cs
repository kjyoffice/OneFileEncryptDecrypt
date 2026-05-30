using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XWork
{
    public class DecryptWork
    {
        private static bool IsAllowSourceAndFinalFilePath(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms, XModel.CryptoWorkOrder cwo, string decryptOriginalFIlePath)
        {
            var workDoneFileExt = XValue.ProcessValue.WorkFileExtension_DoneX.ToUpper();
            var result = false;

            // 원본파일 역시 존재하면 안된다
            if (File.Exists(decryptOriginalFIlePath) == false)
            {
                // 파일 확장자 체크랑 ZIP 파일인지 체크한다
                if ((Path.GetExtension(cwo.SourceFilePath).ToUpper() == workDoneFileExt) && (FileWork.IsZIPFileMagicByte(cwo.SourceFilePath) == true))
                {
                    // OK
                    result = true;
                }
                else
                {
                    // 복호화 파일이 올바르지 않습니다.
                    cwms.Error.MessageNow(asx.WorkMessage.DecryptFileWrong, true);
                }
            }
            else
            {
                // 같은 이름의 암호화된 파일이 있습니다.
                cwms.Error.MessageNow(asx.WorkMessage.AlreadyExistDecryptFile, true);
            }

            return result;
        }

        private static void ZIPExtract(XAppSettings.AppSettingsX asx, XModel.CryptoXFilePath cfn, XModel.CryptoWorkOrder cwo, XModel.ProgressViewer pv)
        {
            // 우선 파일 압축을 해제한다
            FileWork.ZIPExtract(cwo.SourceFilePath, cfn.WorkDirectoryPath, asx.WorkMessage.ZIPExtractFile, pv);
        }

        private static XModel.CryptoInfo GetCryptoInfo(XModel.CryptoXFilePath cfn)
        {
            var jsonText = File.ReadAllText(cfn.CryptoInfoFilePath, Encoding.UTF8);
            var jsonData = JsonWork.ToDataModel<XModel_Json.CryptoInfo_Json>(jsonText);
            var result = new XModel.CryptoInfo(jsonData);

            return result;
        }

        private static void ErrorMessage(XConsole.ConsoleWriteMessageSet cwms, XModel.CryptoXFilePath cfn, string message)
        {
            // 작업이 중단되었으니 압축풀어진 파일들 삭제
            cfn.DeleteAllFile(string.Empty);

            cwms.EmptyLine();
            cwms.Error.MessageNow(message, true);
        }

        // ---------------------------------------------------------------------------------------------

        public static void ExecuteNow(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms, XModel.CryptoWorkOrder cwo)
        {
            var appTitle = XValue.ProcessValue.ApplicationPublicTitle;
            // 복호화 후 원본파일 경로
            var decryptOriginalFIlePath = cwo.CreateDecryptOriginalFIlePath();

            cwms.Success.MessageNow(appTitle);
            cwms.Normal.MessageNow(" - ");
            cwms.Warning.MessageNow("Decrypt", true);
            cwms.EmptyLine();
            cwms.Warning.MessageNow("[Source File] ");
            cwms.Success.MessageNow(cwo.SourceFilePath, true);
            cwms.Warning.MessageNow("[Target File] ");
            cwms.Success.MessageNow(decryptOriginalFIlePath, true);
            cwms.EmptyLine();

            // 소스파일과 작업파일 체크
            if (DecryptWork.IsAllowSourceAndFinalFilePath(asx, cwms, cwo, decryptOriginalFIlePath) == true)
            {
                // 저장 할 파일들 경로생성
                var cfn = asx.CreateCryptoWorkPath();
                var pv = new XModel.ProgressViewer();

                // 우선 파일 압축을 해제한다
                DecryptWork.ZIPExtract(asx, cfn, cwo, pv);

                // 복호화 정보 받아오기
                var ci = DecryptWork.GetCryptoInfo(cfn);

                if (ci.IsAllow == true)
                {
                    // 복호화 필수 파일들이 있는지 체크
                    // 아무래도 zip 파일 경로 아무거나 넣으면 일단 압축을 풀거기 때문에 필수 파일이 모두 있는지 체크함
                    if (cfn.IsAllExistDecryptFile == true)
                    {
                        var cryptoVersion = XValue.ProcessValue.CryptoVersion1;

                        cwms.EmptyLine();
                        cwms.Warning.MessageNow("[Mode] ");
                        cwms.Success.MessageNow(ci.CryptoMode, true);
                        cwms.Warning.MessageNow("[Version] ");
                        cwms.Success.MessageNow(ci.CryptoVersion.ToString(), true);
                        cwms.EmptyLine();

                        // 복호화 방법 체크
                        if (((ci.CryptoMode == XValue.ProcessValue.CryptoMode_AES256CBC) || (ci.CryptoMode == XValue.ProcessValue.CryptoMode_AES256GCM)) && (ci.CryptoVersion == cryptoVersion))
                        {
                            // AES256 CBC / GCM
                            DecryptWork_AES256.ExecuteNow(asx, cwms, cwo, cfn, pv, decryptOriginalFIlePath, ci);
                        }
                        else
                        {
                            // 지정되지 않은 복호화 작업입니다.
                            DecryptWork.ErrorMessage(cwms, cfn, asx.WorkMessage.UndefinedDecryptWork);
                        }
                    }
                    else
                    {
                        // 존재하지 않는 복호화 필수 파일이 있습니다.
                        DecryptWork.ErrorMessage(cwms, cfn, asx.WorkMessage.NotExistDecryptRequireFile);
                    }
                }
                else
                {
                    // 존재하지 않는 복호화 필수 파일이 있습니다.
                    DecryptWork.ErrorMessage(cwms, cfn, asx.WorkMessage.WrongDecryptInfo);
                }
            }
        }
    }
}

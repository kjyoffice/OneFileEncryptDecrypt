using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XWork
{
    public class DecryptWork
    {
        public static void ExecuteNow(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms, XModel.CryptoWorkOrder cwo)
        {
            // 복호화 후 원본파일 경로
            var decryptOriginalFIlePath = cwo.CreateDecryptOriginalFIlePath;

            // 원본파일 역시 존재하면 안된다
            if (File.Exists(decryptOriginalFIlePath) == false)
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
                    // 원본파일 해쉬 읽기
                    var originalChecksumChecker = XCrypto.HashWork.ConvertHashText(File.ReadAllBytes(cfn.OriginalChecksumFilePath));
                    // 암호화 된 파일 해쉬 읽기
                    var encryptDataChecksumChecker = XCrypto.HashWork.ConvertHashText(File.ReadAllBytes(cfn.EncryptDataChecksumFilePath));
                    // 복호화 IV 읽기
                    var cryptoIV = File.ReadAllBytes(cfn.CryptoIVFilePath);
                    // 암호화 된 파일 읽기
                    var encryptData = FileWork.GetFileByte(cfn.EncryptDataFilePath, asx.WorkMessage.ReadFile, pv);
                    // 암호화 된 파일 해쉬 만들기
                    var encryptDataChecksum = XCrypto.HashWork.ConvertHashText(XCrypto.HashWork.CreateSHA512(encryptData, asx.WorkMessage.EncryptChecksum, pv));

                    // 암호화 된 파일 해쉬 비교
                    if (encryptDataChecksum == encryptDataChecksumChecker)
                    {
                        // 복호화 키 생성
                        var cryptoKey = XCrypto.AES256Process.CreateKey(cwo.CryptoPassword, asx.Crypto.GetSalt);
                        // 파일 복호화
                        var decryptData = XCrypto.AES256X.DecryptNow(cryptoKey, cryptoIV, encryptData, asx.WorkMessage.DecryptFile, pv);

                        // 복호화 데이터가 있으면 일단 정상 비번이라고 간주
                        if (decryptData.Length > 0)
                        {
                            // 암호화 된 파일 해쉬 만들기
                            var decryptDataChecksum = XCrypto.HashWork.ConvertHashText(XCrypto.HashWork.CreateSHA512(decryptData, asx.WorkMessage.DecryptChecksum, pv));

                            if (decryptDataChecksum == originalChecksumChecker)
                            {
                                // 원본파일 저장
                                FileWork.WriteFileByte(decryptData, decryptOriginalFIlePath, asx.WorkMessage.SaveDecryptFile, pv);

                                // 작업파일 삭제
                                cfn.DeleteAllFile(cwo.SourceFilePath);

                                cwms.EmptyLine();
                                // 파일을 복호화 했습니다.
                                cwms.Success.MessageNow(asx.WorkMessage.DecryptFileDone);
                            }
                            else
                            {
                                cwms.EmptyLine();
                                // 복호화 파일 해쉬가 다릅니다.
                                cwms.Error.MessageNow(asx.WorkMessage.DifferentDecryptChecksum);
                            }
                        }
                        else
                        {
                            // 복호화 비번 틀림으로 간주
                            cwms.EmptyLine();
                            // 복호화에 실패했습니다.
                            cwms.Error.MessageNow(asx.WorkMessage.DecryptFail);
                        }
                    }
                    else
                    {
                        cwms.EmptyLine();
                        // 암호화 파일 해쉬가 다릅니다.
                        cwms.Error.MessageNow(asx.WorkMessage.DifferentEncryptChecksum);
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
                // 같은 이름의 암호화된 파일이 있습니다.
                // 진행이 중단되었습니다.
                cwms.Error.MessageNow(asx.WorkMessage.AlreadyExistDecryptFile);
            }
        }
    }
}

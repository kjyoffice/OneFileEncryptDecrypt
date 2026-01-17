// AES-GCM
var pw = Encoding.UTF8.GetBytes("hello");
var salt = Encoding.UTF8.GetBytes("world");
var key = XCrypto.AES256Process.CreateKey(pw, salt);
var nonce = XCrypto.AES256Process.CreateNonce;
var plainText = Encoding.UTF8.GetBytes("Hello World");
var aad = Encoding.UTF8.GetBytes("JSON OR TEXT, Want non encrypt data like Header, Info!"); // Optional

var encryptX = new byte[plainText.Length];
var tagX = new byte[16];

// https://learn.microsoft.com/ko-kr/dotnet/api/system.security.cryptography.aesgcm?view=net-10.0
// https://www.scottbrady.io/c-sharp/aes-gcm-dotnet
using (var aesGcm = new AesGcm(key, tagX.Length))
{
    aesGcm.Encrypt(
        nonce,
        plainText,
        encryptX,
        tagX,
        aad
    );
}

// Save!
// nonce
// encryptX
// tagX

var decryptX = new byte[encryptX.Length];

using (var aesGcm = new AesGcm(key, tagX.Length))
{
    aesGcm.Decrypt(
        nonce,
        encryptX,
        tagX,
        decryptX,
        aad
    );
}
// catch (CryptographicException)

Console.WriteLine(Encoding.UTF8.GetString(plainText));
Console.WriteLine(Encoding.UTF8.GetString(decryptX));

// 돌긴 하는데.. GPT왈, 고용량 데이터(Ex 1GB) 암호화는 청크로 잘라서 해란다...
// Nonce도 파일별 하나 받아서 청크별 인덱스 붙여서 중복 nonce 없게 하라고 하고
// 섞이는거 대비 aad에 정보로 청크별 순서도 같이 기록해라네 
// 복호화 역시 청크로 잘린거 복호화 해서 이어 붙여야 한다는건데...
// 이건 나중에 차기 버젼에 하기로 하자 ㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎ ㅠㅠ
// 아니면 바운스캐슬로 해야할듯
// 암튼 나중에~~~
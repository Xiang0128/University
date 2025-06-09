<?php
// 設定預設的帳號與密碼
$correctUsername = "test";
$correctPassword = "test";

// 獲取表單提交的資料
$username = $_POST['username'] ?? '';
$password = $_POST['password'] ?? '';

// 驗證帳號與密碼
if ($username === $correctUsername && $password === $correctPassword) {
    echo "<!DOCTYPE html>
    <html lang='zh-TW'>
    <head>
        <meta charset='UTF-8'>
        <title>帳號密碼正確</title>
    </head>
    <body>
        <h1>登入完成</h1>
    </body>
    </html>";
} else {
    echo "<!DOCTYPE html>
    <html lang='zh-TW'>
    <head>
        <meta charset='UTF-8'>
        <meta http-equiv='refresh' content='1;url=s1114572.html'>
        <title>帳號或密碼錯誤</title>
    </head>
    <body>
        <h1>登入失敗請重新登入</h1>
    </body>
    </html>";
}
?>

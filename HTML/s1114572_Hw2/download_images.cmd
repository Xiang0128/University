@echo off
setlocal enabledelayedexpansion

REM 建立資料夾下載圖片
set "download_folder=images"
if not exist "%download_folder%" mkdir "%download_folder%"

REM 設定開始時間（現在時間減去72小時）
for /f "tokens=1-4 delims=/-:. " %%a in ('powershell -command "(Get-Date).AddHours(-72).ToString('yyyy-MM-dd HH:mm')"') do (
    set "year=%%a"
    set "month=%%b"
    set "day=%%c"
    set "hour=%%d"
)

REM 迴圈下載每小時一張圖片
for /L %%i in (0,1,72) do (
    REM 計算當前時間並格式化
    set /A "current_hour=(hour+%%i) %% 24"
    set /A "days_ahead=(hour+%%i) / 24"
    
    REM 使用 PowerShell 直接生成時間戳，避免 for /f 的變數問題
    for /f %%t in ('powershell -command "(Get-Date -Year !year! -Month !month! -Day !day! -Hour !current_hour! -Minute 0 -Second 0).AddDays(!days_ahead!).ToString('yyyy-MM-dd_HH00')"') do (
        set "formatted_time=%%t"
    )

    REM 檢查 formatted_time 是否生成正確
    echo 嘗試下載網址: https://www.cwa.gov.tw/Data/windspeed/!formatted_time!.GWD.png

    REM 設定圖片的下載網址
    set "download_url=https://www.cwa.gov.tw/Data/windspeed/!formatted_time!.GWD.png"

    REM 使用 wget 下載圖片到指定資料夾，並添加 User-Agent 標頭
    wget --header="User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36" "!download_url!" -P "%download_folder%"
)

echo 下載完成！
pause

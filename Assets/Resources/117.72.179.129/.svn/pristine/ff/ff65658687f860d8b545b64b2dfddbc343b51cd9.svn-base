@echo off
chcp 65001 >nul
setlocal enabledelayedexpansion

echo ========================================
echo         SVN 快速操作脚本
echo ========================================
echo.

:: 设置颜色
set "GREEN=[92m"
set "YELLOW=[93m"
set "RED=[91m"
set "BLUE=[94m"
set "RESET=[0m"

:: 检查SVN
svn --version >nul 2>&1
if errorlevel 1 (
    echo %RED%错误：未找到SVN命令%RESET%
    pause
    exit /b 1
)

:: 检查SVN仓库
if not exist ".svn" (
    echo %RED%错误：当前目录不是SVN仓库%RESET%
    pause
    exit /b 1
)

echo %GREEN%开始执行...%RESET%
echo.

:: 1. 更新（以远程为主）
echo %YELLOW%更新工作副本（以远程为主）...%RESET%
svn update --accept postpone
if errorlevel 1 (
    echo %RED%检测到冲突！%RESET%
    echo.
    echo %BLUE%请选择冲突解决策略：%RESET%
    echo 1. 以远程版本为主（推荐）
    echo 2. 以本地版本为主
    echo 3. 手动解决冲突
    echo 4. 跳过更新，继续执行
    echo.
    set /p choice="请输入选择（1-4）："
    
    if "!choice!"=="1" (
        echo %YELLOW%选择以远程版本为主...%RESET%
        svn update --accept theirs-full
        if errorlevel 1 (
            echo %RED%更新失败%RESET%
            pause
            exit /b 1
        )
        echo %GREEN%更新成功（以远程为主）%RESET%
    ) else if "!choice!"=="2" (
        echo %YELLOW%选择以本地版本为主...%RESET%
        svn update --accept working
        if errorlevel 1 (
            echo %RED%更新失败%RESET%
            pause
            exit /b 1
        )
        echo %GREEN%更新成功（以本地为主）%RESET%
    ) else if "!choice!"=="3" (
        echo %YELLOW%请手动解决冲突...%RESET%
        echo 冲突文件已标记，请手动编辑解决冲突后按任意键继续...
        pause
        svn resolved --accept working .
        echo %GREEN%冲突已手动解决%RESET%
    ) else if "!choice!"=="4" (
        echo %YELLOW%跳过更新，继续执行后续操作...%RESET%
    ) else (
        echo %RED%无效选择，默认以远程版本为主%RESET%
        svn update --accept theirs-full
        if errorlevel 1 (
            echo %RED%更新失败%RESET%
            pause
            exit /b 1
        )
        echo %GREEN%更新成功（以远程为主）%RESET%
    )
) else (
    echo %GREEN%更新成功（无冲突）%RESET%
)
echo.

:: 2. 添加所有新文件
echo %YELLOW%添加新文件...%RESET%
svn add --force . 2>nul
echo %GREEN%文件添加完成%RESET%
echo.

:: 3. 提交
echo %YELLOW%提交更改...%RESET%
svn commit -m "自动提交 - %date% %time%"
if errorlevel 1 (
    echo %RED%提交失败%RESET%
    pause
    exit /b 1
) else (
    echo %GREEN%提交成功%RESET%
)
echo.

echo %GREEN%操作完成！%RESET%
pause
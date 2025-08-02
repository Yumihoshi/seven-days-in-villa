@echo off
chcp 65001 >nul
setlocal enabledelayedexpansion

echo ========================================
echo            SVN 自动化脚本
echo ========================================
echo.

:: 设置颜色
set "GREEN=[92m"
set "YELLOW=[93m"
set "RED=[91m"
set "BLUE=[94m"
set "RESET=[0m"

:: 检查SVN是否安装
svn --version >nul 2>&1
if errorlevel 1 (
    echo %RED%错误：未找到SVN命令，请确保SVN已正确安装并添加到PATH环境变量中%RESET%
    pause
    exit /b 1
)

:: 检查当前目录是否为SVN仓库
if not exist ".svn" (
    echo %RED%错误：当前目录不是SVN仓库%RESET%
    pause
    exit /b 1
)

echo %GREEN%开始执行SVN自动化操作...%RESET%
echo.

:: 1. 更新工作副本 (pull) - 以远程为主
echo %YELLOW%步骤1：更新工作副本（以远程为主）...%RESET%
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
            echo %RED%更新失败，请手动解决冲突后重试%RESET%
            pause
            exit /b 1
        )
        echo %GREEN%更新成功（以远程为主）%RESET%
    ) else if "!choice!"=="2" (
        echo %YELLOW%选择以本地版本为主...%RESET%
        svn update --accept working
        if errorlevel 1 (
            echo %RED%更新失败，请手动解决冲突后重试%RESET%
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
            echo %RED%更新失败，请手动解决冲突后重试%RESET%
            pause
            exit /b 1
        )
        echo %GREEN%更新成功（以远程为主）%RESET%
    )
) else (
    echo %GREEN%更新成功（无冲突）%RESET%
)
echo.

:: 2. 添加新文件和文件夹
echo %YELLOW%步骤2：添加新文件和文件夹...%RESET%
svn add --force .
if errorlevel 1 (
    echo %YELLOW%添加文件时出现警告（可能是文件已存在）%RESET%
)
echo %GREEN%文件添加完成%RESET%
echo.

:: 3. 检查状态
echo %YELLOW%步骤3：检查工作副本状态...%RESET%
svn status
echo.

:: 4. 提交更改
echo %YELLOW%步骤4：提交更改...%RESET%
set /p commit_message="请输入提交信息（直接回车使用默认信息）："
if "!commit_message!"=="" (
    set "commit_message=自动提交 - %date% %time%"
)

svn commit -m "!commit_message!"
if errorlevel 1 (
    echo %RED%提交失败，请检查错误信息%RESET%
    pause
    exit /b 1
) else (
    echo %GREEN%提交成功%RESET%
)
echo.

:: 5. 推送更改（SVN中通常不需要单独的push，但这里可以再次更新确保同步）
echo %YELLOW%步骤5：确保与服务器同步...%RESET%
svn update --accept theirs-full
if errorlevel 1 (
    echo %RED%最终同步失败%RESET%
    pause
    exit /b 1
) else (
    echo %GREEN%同步成功%RESET%
)
echo.

echo %GREEN%========================================
echo 所有操作完成！
echo ========================================%RESET%
echo.

:: 显示最终状态
echo %YELLOW%最终工作副本状态：%RESET%
svn status

pause
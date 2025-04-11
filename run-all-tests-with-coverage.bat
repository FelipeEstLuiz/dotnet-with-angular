@echo off
setlocal enabledelayedexpansion

:: Caminho do projeto de teste
set TEST_PROJECT=tests\Tests\Tests.csproj

:: Caminho do reportgenerator
set REPORTGENERATOR_PATH=%USERPROFILE%\.dotnet\tools\reportgenerator.exe

:: Instala o reportgenerator se não estiver instalado
echo Verificando se reportgenerator esta instalado...
dotnet tool list -g | findstr /C:"reportgenerator" >nul
if errorlevel 1 (
    echo Instalando reportgenerator...
    dotnet tool install -g dotnet-reportgenerator-globaltool
)

:: Limpar resultados antigos
echo Limpando resultados anteriores...
rmdir /s /q TestResults >nul 2>&1
rmdir /s /q coverage-report >nul 2>&1

:: Executa o teste com cobertura
echo Executando testes com cobertura...
dotnet test %TEST_PROJECT% --collect:"XPlat Code Coverage"

:: Juntar todos os arquivos de cobertura
set COVERAGE_FILES=
for /r %%f in (*.cobertura.xml) do (
    set "COVERAGE_FILES=!COVERAGE_FILES!%%f;"
)

:: Remover o último ponto e vírgula
set "COVERAGE_FILES=%COVERAGE_FILES:~0,-1%"

if "%COVERAGE_FILES%"=="" (
    echo Nenhum arquivo de cobertura encontrado.
    exit /b 1
)

:: Verifica se o reportgenerator está disponível no PATH
where reportgenerator >nul 2>&1
if %errorlevel%==0 (
    set REPORTGENERATOR_CMD=reportgenerator
) else (
    set REPORTGENERATOR_CMD=%REPORTGENERATOR_PATH%
)

:: Gera o relatório HTML
echo Gerando relatorio HTML...
%REPORTGENERATOR_CMD% -reports:"%COVERAGE_FILES%" -targetdir:"coverage-report" -reporttypes:Html

:: Verifica se o HTML foi criado
if not exist "coverage-report\index.html" (
    echo O relatorio HTML nao foi gerado.
    exit /b 1
)

:: Abrir o relatório no navegador
echo Abrindo relatorio...
start coverage-report\index.html

echo Finalizado com sucesso!
endlocal

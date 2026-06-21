@ECHO OFF

IF "%1" == "clean" (
    "C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" /nologo Build.xml /t:Clean
    
    GOTO:EOF
)

IF "%1" == "NovoDocenteOnline" (
    IF "%2" == "dev" (
        echo "dev mode >> ZIP generation and PreCompiled application disabled"
        
        "C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" /nologo Build.xml /t:NovoDocenteOnline /property:GenerateBuildArchive=false;GeneratePreCompiled=false;VisualStudioVersion=10.0
        
        GOTO:EOF
    )
    
    "C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" /nologo Build.xml /t:NovoDocenteOnline /property:VisualStudioVersion=10.0
    
    GOTO:EOF
)

IF "%1" == "gestao" (
    IF "%2" == "dev" (
        echo "dev mode >> ZIP generation and PreCompiled application disabled"
        
        "C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" /nologo Build.xml /t:LyceumGov /property:GenerateBuildArchive=false;GeneratePreCompiled=false
        
        GOTO:EOF
    )
    
    "C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" /nologo Build.xml /t:LyceumGov
    
    GOTO:EOF
)


IF "%1" == "dol" (
    IF "%2" == "dev" (
        echo "dev mode >> ZIP generation and PreCompiled application disabled"
        
        "C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" /nologo Build.xml /t:LyceumGovDOL /property:GenerateBuildArchive=false;GeneratePreCompiled=false
        
        GOTO:EOF
    )
    
    "C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" /nologo Build.xml /t:LyceumGovDOL
        
    GOTO:EOF
)

IF "%1" == "processoseletivo" (
    IF "%2" == "dev" (
        echo "dev mode >> ZIP generation and PreCompiled application disabled"
        
        "C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" /nologo Build.xml /t:ProcessoSeletivoAluno /property:GenerateBuildArchive=false;GeneratePreCompiled=false
        
        GOTO:EOF
    )
    
    "C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" /nologo Build.xml /t:ProcessoSeletivoAluno
        
    GOTO:EOF
)

IF "%1" == "aol" (
    IF "%2" == "dev" (
        echo "dev mode >> ZIP generation and PreCompiled application disabled"
        
        "C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" /nologo Build.xml /t:LyceumGovAOL /property:GenerateBuildArchive=false;GeneratePreCompiled=false
        
        GOTO:EOF
    )
    
    "C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" /nologo Build.xml /t:LyceumGovAOL
        
    GOTO:EOF
)

IF "%1" == "biblioteca" (
    IF "%2" == "dev" (
        echo "dev mode >> ZIP generation and PreCompiled application disabled"
        
        "C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" /nologo Build.xml /t:PortalBiblioteca /property:GenerateBuildArchive=false;GeneratePreCompiled=false
        
        GOTO:EOF
    ) 
    
    "C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" /nologo Build.xml /t:PortalBiblioteca
        
    GOTO:EOF
)

IF "%1" == "core" (
    "C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" /nologo Build.xml /t:UpdateCore
    
    GOTO:EOF
)

IF "%1" == "cronos" (
    "C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" /nologo Build.xml /t:UpdateCronos
    
    GOTO:EOF
)

IF "%1" == "dlls" (
    "C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" /nologo Build.xml /t:UpdateDLLs
    
    GOTO:EOF
)

IF "%1" == "reports" (
    "C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" Build.xml /t:UpdateRDLs
    
    GOTO:EOF
)

IF "%1" == "all" (
    "C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" Build.xml /t:All
    
    GOTO:EOF
)

"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" Build.xml /v:minimal
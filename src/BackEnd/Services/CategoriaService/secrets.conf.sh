#!/bin/bash

# Verifica se existe pelo menos um arquivo .csproj na pasta atual ou subpastas
project_file=$(find . -type f -name "*.csproj" | head -n 1)

if [ -z "$project_file" ]; then
    echo "Erro: Nenhum arquivo .csproj encontrado. Verifique se está no diretório correto."
    exit 1
fi

# Obtém o diretório do projeto onde o arquivo .csproj está localizado
project_dir=$(dirname "$project_file")

# Determina o sistema operacional
if [[ "$OSTYPE" == "linux-gnu"* || "$OSTYPE" == "darwin"* ]]; then
    # Linux ou macOS
    CERT_PATH="${APPDATA}/ASP.NET/Https"

    # Limpa os segredos antigos, se existirem
    if dotnet user-secrets list >/dev/null 2>&1; then
        echo "Limpando User Secrets existentes..."
        dotnet user-secrets clear
    fi

    # Inicializa os User Secrets e configura os segredos
    dotnet user-secrets init

    # Defina o caminho do certificado completo (incluir nome do arquivo .pfx)
    CERT_FILE="${CERT_PATH}/categoria-service_dev.pfx"
    dotnet user-secrets set "certificate" "$CERT_FILE"
    dotnet user-secrets set "pwd_certificate" ""  # Ou forneça a senha, se necessário
    dotnet user-secrets set "https_port" "7264"  
    
    # Exibe o conteúdo do diretório de User Secrets
    echo "Conteúdo do diretório de User Secrets:"
    ls -l "$CERT_PATH"

    # Exibe o caminho completo do arquivo de certificado
    echo "Caminho completo do certificado: $CERT_FILE"

elif [[ "$OSTYPE" == "msys" || "$OSTYPE" == "cygwin" || "$OSTYPE" == "win32" ]]; then
    # Windows
    CERT_PATH="${APPDATA}\\ASP.NET\\Https"  # Diretório UserSecrets no Windows

    # Limpa os segredos antigos, se existirem
    if dotnet user-secrets list >/dev/null 2>&1; then
        echo "Limpando User Secrets existentes..."
        dotnet user-secrets clear
    fi

    # Inicializa os User Secrets e configura os segredos
    dotnet user-secrets init

    # Define o caminho completo do arquivo .pfx (substitua pelo nome correto do arquivo)
    CERT_FILE="${CERT_PATH}\\categoria-service_dev.pfx"
    dotnet user-secrets set "certificate" "$CERT_FILE"
    dotnet user-secrets set "pwd_certificate" ""  # Ou forneça a senha, se necessário
    dotnet user-secrets set "https_port" "7001"  

    # Exibe o conteúdo do diretório de User Secrets
    echo "Conteúdo do diretório de User Secrets:"
    ls -l "$CERT_PATH"

    # Exibe o caminho completo do arquivo de certificado
    echo "Caminho completo do certificado: $CERT_FILE"

else
    echo "Sistema operacional não suportado."
    exit 1
fi

echo "Processo concluído."

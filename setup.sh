#!/bin/bash
# Datwise Setup Script for Unix/Linux/macOS
# This script helps set up and run the Datwise application

echo "=========================================="
echo "Datwise Setup and Run Script"
echo "=========================================="
echo ""

# Colors for output
GREEN='\033[0;32m'
BLUE='\033[0;34m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo -e "${YELLOW}Error: .NET SDK is not installed${NC}"
    echo "Please install .NET 9.0 SDK from https://dotnet.microsoft.com/en-us/download"
    exit 1
fi

echo -e "${GREEN}? .NET SDK found$(dotnet --version)${NC}"
echo ""

# Navigate to the root directory
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
cd "$SCRIPT_DIR" || exit 1

echo -e "${BLUE}Step 1: Restoring NuGet packages...${NC}"
dotnet restore
if [ $? -ne 0 ]; then
    echo -e "${YELLOW}Error during restore${NC}"
    exit 1
fi

echo ""
echo -e "${BLUE}Step 2: Building solution...${NC}"
dotnet build
if [ $? -ne 0 ]; then
    echo -e "${YELLOW}Error during build${NC}"
    exit 1
fi

echo ""
echo -e "${GREEN}? Build successful!${NC}"
echo ""
echo -e "${BLUE}=========================================="
echo "Setup complete! To run the application:"
echo "=========================================${NC}"
echo ""
echo -e "${GREEN}Terminal 1 - Start the API:${NC}"
echo "  cd Datwise.Api && dotnet run"
echo ""
echo -e "${GREEN}Terminal 2 - Start the WebForms:${NC}"
echo "  cd Datwise.WebForms && dotnet run"
echo ""
echo -e "${GREEN}Access:${NC}"
echo "  - WebForms UI: https://localhost:7290 or http://localhost:5281"
echo "  - API: https://localhost:7194 or http://localhost:5281"
echo "  - Swagger Docs: https://localhost:7194/swagger"
echo ""

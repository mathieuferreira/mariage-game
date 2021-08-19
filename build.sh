#!/bin/bash

UNITY_PATH=/Users/mathieu/workspace/unity/2020.1.3f1/Unity.app/Contents/MacOS
PROJECT_PATH=/Users/mathieu/workspace/unity/mariage-game

echo "Building Linux x64 Player..."
rm -Rf ${PROJECT_PATH}/Builds/linux64
mkdir ${PROJECT_PATH}/Builds/linux64
${UNITY_PATH}/Unity -quit -batchmode -projectPath ${PROJECT_PATH} -buildLinux64Player ${PROJECT_PATH}/Builds/linux64/mariage-game-linux64
Echo "Build completed !"

echo "Building Windows x64 Player..."
rm -Rf ${PROJECT_PATH}/Builds/win64
mkdir ${PROJECT_PATH}/Builds/win64
${UNITY_PATH}/Unity -quit -batchmode -projectPath ${PROJECT_PATH} -buildWindows64Player ${PROJECT_PATH}/Builds/win64/mariage-game-win64
echo "Build completed !"
echo "Zipping..."
rm ${PROJECT_PATH}/Builds/win64.zip
zip -r ${PROJECT_PATH}/Builds/win64.zip ${PROJECT_PATH}/Builds/win64

echo "Building Windows x32 Player..."
rm -Rf ${PROJECT_PATH}/Builds/win32
mkdir ${PROJECT_PATH}/Builds/win32
${UNITY_PATH}/Unity -quit -batchmode -projectPath ${PROJECT_PATH} -buildWindowsPlayer ${PROJECT_PATH}/Builds/win32/mariage-game-win32
echo "Build completed !"
echo "Zipping..."
rm ${PROJECT_PATH}/Builds/win32.zip
zip -r ${PROJECT_PATH}/Builds/win32.zip ${PROJECT_PATH}/Builds/win32

echo "Building MacOS Player..."
rm -Rf ${PROJECT_PATH}/Builds/macOS
mkdir ${PROJECT_PATH}/Builds/macOS
${UNITY_PATH}/Unity -quit -batchmode -projectPath ${PROJECT_PATH} -buildOSXUniversalPlayer ${PROJECT_PATH}/Builds/macOS/mariage-game-mac
Echo "Build completed !"

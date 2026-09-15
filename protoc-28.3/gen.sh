#!/bin/bash
# aaa.bat 的 macOS / Linux 版本
# 用法：在终端里 cd 到本文件所在目录，然后执行  ./gen.sh
cd "$(dirname "$0")"

mkdir -p out

shopt -s nullglob
files=(proto/*.proto)
if [ ${#files[@]} -eq 0 ]; then
  echo "proto/ 目录里没有 .proto 文件，先把你的 .proto 放进去。"
  exit 1
fi

for f in "${files[@]}"; do
  echo "正在生成: $(basename "$f")"
  ./bin/protoc -I=proto --csharp_out=out "$f" || exit 1
done

echo "全部完成，生成的 .cs 文件在 out/ 目录里。"

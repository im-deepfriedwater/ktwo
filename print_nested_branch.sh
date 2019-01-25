echo "Printing out the nested repository's branch..."
cwd=$(pwd)
cd unity-ktwo/Assets/ExternalAssets/assets-ktwo
echo "Branch at path: $(pwd)"
git branch
cd "$cwd"

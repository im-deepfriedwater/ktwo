echo "Printing out the git status nested repository..."
cwd=$(pwd)
cd unity-ktwo/Assets/ExternalAssets/assets-ktwo
echo "git status at path: $(pwd)"
git status
cd $cwd
echo "!!!Be sure to CD into the nested repository before you make changes!!!"
echo "_____________________"


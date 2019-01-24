cwd=$(pwd)
echo "Please enter the name of the branch to switch the inner one to."
read -p "Name of the branch: " input
cd unity-ktwo/Assets/ExternalAssets/assets-ktwo
git checkout $input
cd $cwd

cwd=$(pwd)
echo "Please run this while in the root directory of the repository."
read -p "What branch do you want to create on all repos? [branch name]: " branchName

while true; do
 read -p "Create $branchName on all repos? [y\n]: " yn
    case $yn in
        [Yy]* ) git checkout -b "$branchName"
        cd unity-ktwo/assets/ExternalAssets/assets-ktwo
        git checkout -b "$branchName"
        cd "$cwd"
        break;;
        [Nn]* ) echo "Okie, cancelling..." && break;;
        * ) echo "Please enter: [y\n]";;
    esac
done

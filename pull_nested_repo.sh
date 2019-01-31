while true; do
    cwd=$(pwd)
    echo "Please run this while in the root directory of the repository."
    read -p "Pull on nested repo? [y\n]: " yn
    case $yn in
        [Yy]* ) cd unity-ktwo/assets/ExternalAssets/assets-ktwo && git pull && cd "$cwd" && break;;
        [Nn]* ) echo "Okie, cancelling..." && break;;
        * ) echo "Please enter: [y\n]";;
    esac
done

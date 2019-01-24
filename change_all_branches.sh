while true; do
    cwd=$(pwd)
    echo "Please run this while in the root directory of the repository."
    read -p "Change ? [y\n]: " yn
    case $yn in
        [Yy]* ) 
            git pull  
            cd unity-ktwo/assets/ExternalAssets/assets-ktwo
            git pull
            cd $cwd
            exit;;
        [Nn]* ) echo "Okie, cancelling..." && exit;;
        * ) echo "Please enter: [y\n]";;
    esac
done
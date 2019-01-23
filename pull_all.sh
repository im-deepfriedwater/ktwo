while true; do
    read -p "Pull on current branches for all repositories? [y\n]: " yn
    case $yn in
        [Yy]* ) git pull && cd unity/ktwo/assets/ExternalAssets/ktwo-assets && git pull && cd ../../../../ && exit;;
        [Nn]* ) echo "Okie, cancelling..." && exit;;
        * ) echo "Please enter: [y\n]";;
    esac
done
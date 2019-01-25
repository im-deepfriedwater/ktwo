while true; do
    cwd=$(pwd)
    instructions="This script will switch both branches to master. "
    echo "Please run this while in the root directory of the repository."
    echo $instructions
    read -p "Continue? [y\n]:" yn
    case $yn in
        [Yy]* ) 
            echo "Changing to both branches to master..."
            git checkout master
            cd unity-ktwo/Assets/ExternalAssets/assets-ktwo
            git checkout master
            cd "$cwd"
            echo "Done!"
            break
            ;;
        [Nn]* ) 
            echo "Okie, cancelling..." && break
            ;;
        * )  
            echo ""
            ;;
    esac
done
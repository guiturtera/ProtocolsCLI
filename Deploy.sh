#!"/bin/bash"

fail_script () {
	echo ""
	echo "Script failed at!"
	echo  "${@}"
	read
	exit 1
}

check_failure () {
	if "$@"; then
		echo Success
	else
		echo $?
		fail_script $@
	fi
	#exit_code=$1
	#if [ ! $exit_code == 0 ]; then
	#	fail_script
	#fi
}

copy_folder () {
	robocopy ${1} ${2} //e //xo //NFL //NDL //NJH //NJS //nc
	
	exit_code=$?
	if [ "$exit_code" -ge "8" ]; then
		echo $exit_code
		fail_script "robocopy"
	else
		echo "Success!"
	fi
}

run_update_package () {
	$1 "source_path=$2" "destiny_path=$3" "package_name=$4" "device_name=$5" "overwrite=$6"
}

create_folder_if_not_exists () {
	folder="${1}"
	if [ ! -d "${folder}" ]; then
		mkdir "${folder}"
	fi
}

clear_folder () {
	folder="${1}"
	rm -rf --force "${folder}"
	mkdir "${folder}"
}

#### PRESYS ADVANCED AUTOMATION SCRIPT ####
#### CHANGE THOSE VARIABLES IN ORDER TO CONFIGURE YOUR PROJECT ####
flick_proj="C:/Users/guilherme.turtera/Desktop/au/src" #Path to FLICK project src
repo_path=$(pwd) #Absolute path of your repo to deploy
worktree_libraries_path="P:/Advanced-Calibrators/Libraries"
project_name="SerialProtocolAbstraction"

#### ####
artifacts_path="${repo_path}/artifacts"
create_folder_if_not_exists "${artifacts_path}"
clear_folder "${artifacts_path}"

release_path="${repo_path}/src/SerialProtocolAbstraction/bin/Release"
version_info_path="${repo_path}/versioninfo.txt"
unit_test_path="${repo_path}/tests/UnitTests/bin/Release/UnitTests.exe"

cd $flick_proj
source ".venv/Scripts/activate"

### BUILD ###
echo " --------------------------- Starting to build --------------------------- "

echo "Building src"
check_failure flick build msbuild -r $repo_path "src/{project_name}"

echo "Building tests..."
check_failure flick build msbuild -r $repo_path "tests/{project_name}"

echo " --------------------------- Success to build --------------------------- "
### BUILD ###


### DEPLOY ###
echo " --------------------------- Starting deployment --------------------------- "

echo "Copying Release into artifacts"
copy_folder "${release_path}" "${artifacts_path}"

echo " --------------------------- Success to deploy --------------------------- "
### DEPLOY ###

### TESTS ###
echo " --------------------------- Starting tests --------------------------- "
echo "Running unit tests..."
check_failure flick nunit unit $unit_test_path

echo " --------------------------- Success to run tests--------------------------- "
### TESTS ###

### VERSIONING ###
echo " --------------------------- Starting versioning --------------------------- "

echo "Starting git release and changelog..."
check_failure flick git deploy $repo_path
version=`cat $version_info_path` # version is updated after git release...

create_folder_if_not_exists "${worktree_libraries_path}/${project_name}"
target_path="${worktree_libraries_path}/${project_name}/${version}"

create_folder_if_not_exists "${target_path}"
clear_folder "${target_path}"
copy_folder "${artifacts_path}" "${target_path}"

#git push origin Shared/${project_name}

echo " -------------------------------- Success to version -------------------------------- "
### VESIONING ###

read

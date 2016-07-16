#!/bin/bash
app="$1"
dir=$(dirname "$0")
pushd "$dir/$1" && {
	if docker ps | grep -q "docker_app_$1";
	then
		./run.sh
	else
		./build.sh
	fi
}

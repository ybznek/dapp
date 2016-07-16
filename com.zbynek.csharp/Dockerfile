FROM ubuntu:16.04

RUN apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF

RUN echo "deb http://download.mono-project.com/repo/debian wheezy main" | tee /etc/apt/sources.list.d/mono-xamarin.list

RUN \
	apt-get -y update	;\
	apt-get -y upgrade	;\
	apt-get -y install git libgtk2.0 libnotify-bin libgconf2-4 libnss3	\
	wget \
	mono-complete mono-xsp unzip curl

RUN curl  https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.sh > ./dnvminstall.sh;  DNX_BRANCH=dev sh ./dnvminstall.sh; bash /root/.dnx/dnvm/dnvm.sh

RUN bash /root/.dnx/dnvm/dnvm.sh install latest

RUN wget http://download.unity3d.com/download_unity/unity-editor-5.1.0f3+2015082501_amd64.deb

RUN dpkg -i unity-editor*deb

RUN apt-get -y install gtk2-engines-murrine docker.io

EXPOSE 1-65535

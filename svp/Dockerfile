FROM ubuntu:16.04

RUN apt-get -y update; apt-get -y upgrade

RUN apt-get install -y sudo wget

RUN wget http://www.svp-team.com/files/svp4-linux-64.tbz2 -O ./svp.tbz2

RUN apt-get update
RUN apt-get install -y sudo bzip2 python3-software-properties software-properties-common


RUN add-apt-repository ppa:djcj/vapoursynth ; apt-get update
RUN apt-get install -y mpv mediainfo libx11-6 libx11-xcb1 libfontconfig1 qt5-default libqt5script5 libmediainfo0v5

RUN apt-get install -y alsa-base  alsa-utils    alsa-oss     flac 

RUN add-apt-repository ppa:rvm/smplayer 
RUN apt-get update
RUN apt-get install -y smplayer smplayer-themes smplayer-skins vlc
RUN tar -jxvf ./svp.tbz2

RUN mkdir -p /root/.config/smplayer/; echo '[advanced]\nmplayer_additional_options="--input-ipc-server=/tmp/mpvsocket --hwdec=no"' >> /root/.config/smplayer/smplayer.ini


FROM fedora:24

RUN dnf install -y dbus-devel GConf2-devel libacl-devel libblkid-devel libcap-devel libcurl-devel libgcrypt-devel libselinux-devel libxml2-devel libxslt-devel make openldap-devel pcre-devel perl-XML-Parser perl-XML-XPath perl-devel python-devel rpm-devel swig bzip2-devel git libtool bzip2 bzip2-devel doxygen asciidoctor; dnf clean all

RUN mkdir -p <<dir>>; cd <<dir>>; git clone https://github.com/OpenSCAP/openscap.git -b maint-1.2

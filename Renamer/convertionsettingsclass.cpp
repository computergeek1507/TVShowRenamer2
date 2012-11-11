#include "convertionsettingsclass.h"

ConvertionSettingsClass::ConvertionSettingsClass(QWidget *parent) :
     QDialog( parent ),
	  QSettings( "ScottNation", "TVShowRenamer", parent )
{
	ui.setupUi( this );
	_ApplicationMajor   = 3;
	_ApplicationMinor   = 0;
	_ApplicationVersion = 0;
}

void ConvertionSettingsClass::load()
{
	QString MainTheme = "Windows";
	int WinVer = QSysInfo::windowsVersion();
	
	switch(WinVer)
	{
		case QSysInfo::WV_2003:
		case QSysInfo::WV_XP:
			MainTheme = "WindowsXP";
		break;
		case QSysInfo::WV_WINDOWS7:
		case QSysInfo::WV_VISTA:
			MainTheme = "WindowsVista";
		break;
	}

	beginGroup( "AppVersion" );
		int TempApplicationMajor   = value( "AppMajor"  , 0 ).toInt();
		int TempApplicationMinor   = value( "AppMinor"  , 0 ).toInt();
		int TempApplicationVersion = value( "AppVersion", 0 ).toInt();
	endGroup(); // ApplicationSettings

	if( ApplicationMajor()   != TempApplicationMajor || 
		ApplicationMinor()   != TempApplicationMinor || 
		ApplicationVersion() != TempApplicationVersion  )
	{
		QDesktopServices::openUrl( QUrl::fromLocalFile( "Changelog.txt" ) );
	}
}

void ConvertionSettingsClass::save()
{
	beginGroup( "AppVersion" );
		setValue( "AppMajor"   , _ApplicationMajor   );
		setValue( "AppMinor"   , _ApplicationMinor   );
		setValue( "AppVersion" , _ApplicationVersion );
	endGroup(); 
}
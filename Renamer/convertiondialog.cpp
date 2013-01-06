#include "convertiondialog.h"
#include "ui_convertiondialog.h"

ConvertionDialog::ConvertionDialog(QWidget *parent) :
	QDialog(parent),
	ui(new Ui::ConvertionDialog)
{
	ui->setupUi(this);
	ui->ShowNameComboBox      ->addItems(QStringList()<<"Show Name"<<"Show name"<<"SHOW NAME"<<"show name");
	ui->SeasonDashComboBox    ->addItems(QStringList()<<" "<<"-");
	ui->EpisodeFormatComboBox ->addItems(QStringList()<<"1x01"<<"0101"<<"S01E01"<<"101"<<"1-1-2013"<<"None");
	ui->TitleDashComboBox     ->addItems(QStringList()<<" "<<"-");
	ui->TitleFormatComboBox   ->addItems(QStringList()<<"Original"<<"Episode Title"<<"Episode title"<<"EPISODE TITLE"<<"episode title"<<"None");
	ui->ExtFormatComboBox     ->addItems(QStringList()<<".ext"<<".Ext"<<".EXT");
	ui->TitleGetComboBox      ->addItems(QStringList()<<"Use File then Online"<<"Use File"<<"Online Only");
	ui->TitleSearchComboBox   ->addItems(QStringList()<<"TVDB.com"<<"TVRage.com"<<"Epguides.com"<<"theXEM.de");
	ui->listWidget->addItem("Default");
}

ConvertionDialog::~ConvertionDialog()
{
	delete ui;
}

QHash<QString, QVariant> ConvertionDialog::GetConvertionSettings()
{
	QHash<QString, QVariant> ConvertionSettings;

	ConvertionSettings.insert( ui->ShowNameComboBox      ->objectName(), ui->ShowNameComboBox      ->currentIndex());
	ConvertionSettings.insert( ui->EpisodeFormatComboBox ->objectName(), ui->EpisodeFormatComboBox ->currentIndex());
	ConvertionSettings.insert( ui->TitleFormatComboBox   ->objectName(), ui->TitleFormatComboBox   ->currentIndex());
	ConvertionSettings.insert( ui->ExtFormatComboBox     ->objectName(), ui->ExtFormatComboBox     ->currentIndex());
	ConvertionSettings.insert( ui->TitleGetComboBox      ->objectName(), ui->TitleGetComboBox      ->currentIndex());
	ConvertionSettings.insert( ui->TitleSearchComboBox   ->objectName(), ui->TitleSearchComboBox   ->currentIndex());
	ConvertionSettings.insert( ui->SeasonDashComboBox    ->objectName(), ui->SeasonDashComboBox    ->currentIndex());
	ConvertionSettings.insert( ui->TitleDashComboBox     ->objectName(), ui->TitleDashComboBox     ->currentIndex());

	ConvertionSettings.insert( ui->SeasonOffsetSpinBox     ->objectName(), ui->SeasonOffsetSpinBox  ->value());
	ConvertionSettings.insert( ui->EpisodeOffsetSpinBox    ->objectName(), ui->EpisodeOffsetSpinBox ->value());

	ConvertionSettings.insert( ui->SpacerCheckBox          ->objectName(), ui->SpacerCheckBox          ->isChecked());
	ConvertionSettings.insert( ui->UnderScoreCheckBox      ->objectName(), ui->UnderScoreCheckBox      ->isChecked());
	ConvertionSettings.insert( ui->RemoveBracketsCheckBox  ->objectName(), ui->RemoveBracketsCheckBox  ->isChecked());
	ConvertionSettings.insert( ui->RemoveDashCheckBox      ->objectName(), ui->RemoveDashCheckBox      ->isChecked());
	ConvertionSettings.insert( ui->RemoveYearCheckBox      ->objectName(), ui->RemoveYearCheckBox      ->isChecked());
	ConvertionSettings.insert( ui->RemoveExtraJunkCheckBox ->objectName(), ui->RemoveExtraJunkCheckBox ->isChecked());
	ConvertionSettings.insert( ui->AutoTitleCheckBox       ->objectName(), ui->AutoTitleCheckBox       ->isChecked());
	ConvertionSettings.insert( ui->OnlineShowCheckBox      ->objectName(), ui->OnlineShowCheckBox      ->isChecked());
	
	return ConvertionSettings;
}

void ConvertionDialog::SetConvertionSettings(QHash<QString, QVariant> ConvertionSettings)
{
	ui->ShowNameComboBox      ->setCurrentIndex( ConvertionSettings[ ui->ShowNameComboBox      ->objectName() ].toInt() );
	ui->EpisodeFormatComboBox ->setCurrentIndex( ConvertionSettings[ ui->EpisodeFormatComboBox ->objectName() ].toInt() );
	ui->TitleFormatComboBox   ->setCurrentIndex( ConvertionSettings[ ui->TitleFormatComboBox   ->objectName() ].toInt() );
	ui->ExtFormatComboBox     ->setCurrentIndex( ConvertionSettings[ ui->ExtFormatComboBox     ->objectName() ].toInt() );
	ui->TitleGetComboBox      ->setCurrentIndex( ConvertionSettings[ ui->TitleGetComboBox      ->objectName() ].toInt() );
	ui->TitleSearchComboBox   ->setCurrentIndex( ConvertionSettings[ ui->TitleSearchComboBox   ->objectName() ].toInt() );
	ui->SeasonDashComboBox    ->setCurrentIndex( ConvertionSettings[ ui->SeasonDashComboBox    ->objectName() ].toInt() );
	ui->TitleDashComboBox     ->setCurrentIndex( ConvertionSettings[ ui->TitleDashComboBox     ->objectName() ].toInt() );
	
	ui->SpacerCheckBox          ->setChecked( ConvertionSettings[ ui->SpacerCheckBox          ->objectName() ].toBool() );
	ui->UnderScoreCheckBox      ->setChecked( ConvertionSettings[ ui->UnderScoreCheckBox      ->objectName() ].toBool() );
	ui->RemoveBracketsCheckBox  ->setChecked( ConvertionSettings[ ui->RemoveBracketsCheckBox  ->objectName() ].toBool() );
	ui->RemoveDashCheckBox      ->setChecked( ConvertionSettings[ ui->RemoveDashCheckBox      ->objectName() ].toBool() );
	ui->RemoveYearCheckBox      ->setChecked( ConvertionSettings[ ui->RemoveYearCheckBox      ->objectName() ].toBool() );
	ui->RemoveExtraJunkCheckBox ->setChecked( ConvertionSettings[ ui->RemoveExtraJunkCheckBox ->objectName() ].toBool() );
	ui->AutoTitleCheckBox       ->setChecked( ConvertionSettings[ ui->AutoTitleCheckBox       ->objectName() ].toBool() );
	ui->OnlineShowCheckBox      ->setChecked( ConvertionSettings[ ui->OnlineShowCheckBox      ->objectName() ].toBool() );
	
	ui->SeasonOffsetSpinBox  ->setValue( ConvertionSettings[ ui->SeasonOffsetSpinBox  ->objectName() ].toInt() );
	ui->EpisodeOffsetSpinBox ->setValue( ConvertionSettings[ ui->EpisodeOffsetSpinBox ->objectName() ].toInt() );
}

void ConvertionDialog::on_OKPushButton_clicked()
{
	accept();
}

void ConvertionDialog::on_CancelPushButton_clicked()
{
	reject();
}

const int& ConvertionDialog::TVShowFormat     () const { return ui->ShowNameComboBox      ->currentIndex(); }
const int& ConvertionDialog::SeasonFormat     () const { return ui->EpisodeFormatComboBox ->currentIndex(); }
const int& ConvertionDialog::TitleFormat      () const { return ui->TitleFormatComboBox   ->currentIndex(); }
const int& ConvertionDialog::ExtFormat        () const { return ui->ExtFormatComboBox     ->currentIndex(); }
const int& ConvertionDialog::TitleGetSetting  () const { return ui->TitleGetComboBox      ->currentIndex(); }
const int& ConvertionDialog::TitleGetLocation () const { return ui->TitleSearchComboBox   ->currentIndex(); }

const bool& ConvertionDialog::SpaceAsSeporator   () const { return ui->SpacerCheckBox          ->isChecked(); }
const bool& ConvertionDialog::ConvertUnderScores () const { return ui->UnderScoreCheckBox      ->isChecked(); }
const bool& ConvertionDialog::RemoveBrackets     () const { return ui->RemoveBracketsCheckBox  ->isChecked(); }
const bool& ConvertionDialog::RemoveDashes       () const { return ui->RemoveDashCheckBox      ->isChecked(); }
const bool& ConvertionDialog::RemoveYear         () const { return ui->RemoveYearCheckBox      ->isChecked(); }
const bool& ConvertionDialog::RemoveJunk         () const { return ui->RemoveExtraJunkCheckBox ->isChecked(); }
const bool& ConvertionDialog::AutoGetTitle       () const { return ui->AutoTitleCheckBox       ->isChecked(); }
const bool& ConvertionDialog::UseOnlineShowName  () const { return ui->OnlineShowCheckBox      ->isChecked(); }

bool ConvertionDialog::SeasonDash      () 
{
	if( ui->SeasonDashComboBox  ->currentIndex()==0 )
		return false;
	else 
		return true;
}
bool ConvertionDialog::TitleDash       ()
{
	//return   ui->TitleDashComboBox   ->currentIndex();
	if( ui->TitleDashComboBox  ->currentIndex() == 0 )
		return false;
	else 
		return true;
}

const int& ConvertionDialog::SeasonOffset  () const { return ui->SeasonOffsetSpinBox->value(); }
const int& ConvertionDialog::EpisodeOffset () const { return ui->EpisodeOffsetSpinBox->value(); }

void ConvertionDialog::setTVShowFormat     ( const int&  TVShowFormat     ){ ui->ShowNameComboBox      ->setCurrentIndex( TVShowFormat );     }
void ConvertionDialog::setSeasonFormat     ( const int&  SeasonFormat     ){ ui->EpisodeFormatComboBox ->setCurrentIndex( SeasonFormat );     }
void ConvertionDialog::setTitleFormat      ( const int&  TitleFormat      ){ ui->TitleFormatComboBox   ->setCurrentIndex( TitleFormat );      }
void ConvertionDialog::setExtFormat        ( const int&  ExtFormat        ){ ui->ExtFormatComboBox     ->setCurrentIndex( ExtFormat );        }
void ConvertionDialog::setTitleGetSetting  ( const int&  TitleGetSetting  ){ ui->TitleGetComboBox      ->setCurrentIndex( TitleGetSetting );  }
void ConvertionDialog::setTitleGetLocation ( const int&  TitleGetLocation ){ ui->TitleSearchComboBox   ->setCurrentIndex( TitleGetLocation ); }

void ConvertionDialog::setSeasonDash       ( bool SeasonDash       )
{
	if(SeasonDash)
		ui->SeasonDashComboBox    ->setCurrentIndex( 1 );       
	else
		ui->SeasonDashComboBox    ->setCurrentIndex( 0 );
}

void ConvertionDialog::setTitleDash        ( bool TitleDash        )
{
	if(TitleDash)
		ui->TitleSearchComboBox   ->setCurrentIndex( 1 );      
	else
		ui->TitleSearchComboBox   ->setCurrentIndex( 0 );
}

void ConvertionDialog::setSpaceAsSeporator   ( const bool& SpaceAsSeporator   ){ ui->SpacerCheckBox->setChecked( SpaceAsSeporator);   }
void ConvertionDialog::setConvertUnderScores ( const bool& ConvertUnderScores ){ ui->SpacerCheckBox->setChecked( ConvertUnderScores); }
void ConvertionDialog::setRemoveBrackets     ( const bool& RemoveBrackets     ){ ui->SpacerCheckBox->setChecked( RemoveBrackets);     }
void ConvertionDialog::setRemoveDashes       ( const bool& RemoveDashes       ){ ui->SpacerCheckBox->setChecked( RemoveDashes);       }
void ConvertionDialog::setRemoveYear         ( const bool& RemoveYear         ){ ui->SpacerCheckBox->setChecked( RemoveYear);         }
void ConvertionDialog::setRemoveJunk         ( const bool& RemoveJunk         ){ ui->SpacerCheckBox->setChecked( RemoveJunk);         }
void ConvertionDialog::setAutoGetTitle       ( const bool& AutoGetTitle       ){ ui->SpacerCheckBox->setChecked( AutoGetTitle);       }
void ConvertionDialog::setUseOnlineShowName  ( const bool& UseOnlineShowName  ){ ui->SpacerCheckBox->setChecked( UseOnlineShowName);  }

void ConvertionDialog::setSeasonOffset  ( const int&  SeasonOffset  ){ ui->SeasonOffsetSpinBox->setValue( SeasonOffset );  }
void ConvertionDialog::setEpisodeOffset ( const int&  EpisodeOffset ){ ui->EpisodeOffsetSpinBox->setValue( EpisodeOffset); }


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
const bool& ConvertionDialog::SeasonDash      () const { return !!ui->SeasonDashComboBox  ->currentIndex(); }
const bool& ConvertionDialog::TitleDash       () const { return   ui->TitleDashComboBox   ->currentIndex(); }

void ConvertionDialog::setTVShowFormat     ( const int&  TVShowFormat     ){ ui->ShowNameComboBox      ->setCurrentIndex( TVShowFormat );     }
void ConvertionDialog::setSeasonFormat     ( const int&  SeasonFormat     ){ ui->EpisodeFormatComboBox ->setCurrentIndex( SeasonFormat );     }
void ConvertionDialog::setTitleFormat      ( const int&  TitleFormat      ){ ui->TitleFormatComboBox   ->setCurrentIndex( TitleFormat );      }
void ConvertionDialog::setExtFormat        ( const int&  ExtFormat        ){ ui->ExtFormatComboBox     ->setCurrentIndex( ExtFormat );        }
void ConvertionDialog::setTitleGetSetting  ( const int&  TitleGetSetting  ){ ui->TitleGetComboBox      ->setCurrentIndex( TitleGetSetting );  }
void ConvertionDialog::setTitleGetLocation ( const int&  TitleGetLocation ){ ui->TitleSearchComboBox   ->setCurrentIndex( TitleGetLocation ); }
void ConvertionDialog::setSeasonDash       ( const bool& SeasonDash       ){ ui->SeasonDashComboBox    ->setCurrentIndex( SeasonDash );       }
void ConvertionDialog::setTitleDash        ( const bool& TitleDash        ){ ui->TitleSearchComboBox   ->setCurrentIndex( TitleDash );        }

#include "convertiondialog.h"
#include "ui_convertiondialog.h"

ConvertionDialog::ConvertionDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::ConvertionDialog)
{
    ui->setupUi(this);
    ui->ShowNameComboBox->addItems(QStringList()<<"Show Name"<<"Show name"<<"SHOW NAME"<<"show name");
    ui->SeasonDashComboBox->addItems(QStringList()<<"-"<<" ");
    ui->EpisodeFormatComboBox->addItems(QStringList()<<"1x01"<<"0101"<<"S01E01"<<"101"<<"1-1-2013"<<"None");
    ui->TitleDashComboBox->addItems(QStringList()<<"-"<<" ");
    ui->TitleFormatComboBox->addItems(QStringList()<<"Original"<<"Episode Title"<<"Episode title"<<"EPISODE TITLE"<<"episode title"<<"None");
    ui->ExtFormatComboBox->addItems(QStringList()<<".ext"<<".Ext"<<".EXT");
    ui->TitleGetComboBox->addItems(QStringList()<<"Use File then Online"<<"Use File"<<"Online Only");
    ui->TitleSearchComboBox->addItems(QStringList()<<"TVDB.com"<<"TVRage.com"<<"Epguides.com"<<"theXEM.de");
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

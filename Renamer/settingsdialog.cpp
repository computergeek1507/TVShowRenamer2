#include "settingsdialog.h"
#include "ui_settingsdialog.h"

SettingsDialog::SettingsDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::SettingsDialog)
{
    ui->setupUi(this);
    ui->comboBox->addItems(QStyleFactory::keys ());
    QApplication::style()->objectName();
    QString styleInfo = this->style()->objectName();
    if(styleInfo.contains("gtk+",Qt::CaseInsensitive)||styleInfo.contains("cde",Qt::CaseInsensitive))
        styleInfo=styleInfo.toUpper();
    else
        styleInfo[0]=styleInfo[0].toUpper();

    //QMessageBox myBox;
    //myBox.setText(styleInfo);
    //myBox.exec();
    ui->comboBox->setCurrentIndex(ui->comboBox->findText(styleInfo));
    connect(ui->comboBox, SIGNAL(currentIndexChanged(QString)), this, SLOT(ChangeStyle(QString)));
}

void SettingsDialog::ChangeStyle(QString text)
{
    QApplication::setStyle(text);
}

SettingsDialog::~SettingsDialog()
{
    delete ui;
}

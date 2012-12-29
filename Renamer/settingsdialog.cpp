#include "settingsdialog.h"
#include "ui_settingsdialog.h"

SettingsDialog::SettingsDialog(QWidget *parent) :
	QDialog(parent),
	ui(new Ui::SettingsDialog)
{
    ui->setupUi(this);
	QStringList StyleKeys = QStyleFactory::keys ();
	for(int i=0;i<StyleKeys.size();i++)
		StyleKeys[i] = StyleKeys[i].toUpper();
	ui->comboBox->addItems(StyleKeys);
    QApplication::style()->objectName();
    QString styleInfo = this->style()->objectName();
	styleInfo=styleInfo.toUpper();

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

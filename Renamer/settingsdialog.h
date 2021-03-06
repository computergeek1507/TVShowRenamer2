#ifndef SETTINGSDIALOG_H
#define SETTINGSDIALOG_H

#include <QDialog>
#include <QStyleFactory>
#include <QMessageBox>


namespace Ui
{
	class SettingsDialog;
}

class SettingsDialog : public QDialog
{
	Q_OBJECT

public:
	explicit SettingsDialog(QWidget *parent = 0);
	~SettingsDialog();

public slots:
	void ChangeStyle(QString text);

private:
	Ui::SettingsDialog *ui;
};

#endif // SETTINGSDIALOG_H

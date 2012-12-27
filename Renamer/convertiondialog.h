#ifndef CONVERTIONDIALOG_H
#define CONVERTIONDIALOG_H

#include <QDialog>

namespace Ui {
class ConvertionDialog;
}

class ConvertionDialog : public QDialog
{
    Q_OBJECT
    
public:
    explicit ConvertionDialog(QWidget *parent = 0);
    ~ConvertionDialog();
    
private:
    Ui::ConvertionDialog *ui;
};

#endif // CONVERTIONDIALOG_H

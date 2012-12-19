#include "mainwindow.h"
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent) :
	QMainWindow(parent),
	ui(new Ui::MainWindow)
{
	//ui->tableViewTVShowList->setModel(new TVShowModel(ui->tableViewTVShowList));
	ui->setupUi(this);
	_TVShowModelList =  new TVShowModel(this);
	proxyModel = new QSortFilterProxyModel(this);
	proxyModel->setSourceModel(_TVShowModelList);
	proxyModel->setDynamicSortFilter(true);

	ui->tableViewTVShowList->setModel(proxyModel);
	ui->tableViewTVShowList->resizeColumnsToContents();
	//setStyleSheet("QMainWindow {background: 'light blue';}");
	_UsableEXT<<"avi"<<"mp4"<<"mkv";
}

MainWindow::~MainWindow()
{
	delete ui;
}

void MainWindow::on_actionAdd_Files_triggered()
{
	QStringList files = QFileDialog::getOpenFileNames(
							this,
							"Select Media File to Add",
							QDir::homePath(),
							"Videos (*."+_UsableEXT.join(" *.")+")");
	Q_FOREACH(QString filename,files)
	{
		QDir fullinfo(filename);
		QFileInfo fi(filename);
		_TVShowModelList->addTVShowItem(TVShowClass(fullinfo.toNativeSeparators(fi.absoluteDir().path()), fi.fileName(),fi.completeSuffix()));
	}
    ConvertFileName();
}

void MainWindow::on_actionAdd_Folder_triggered()
{
	QString dir = QFileDialog::getExistingDirectory(this,"Open Directory",
													QDir::homePath(),
													QFileDialog::ShowDirsOnly
													| QFileDialog::DontResolveSymlinks);
	if(dir=="")return;
	RecurseDirectory(dir);
}

void MainWindow::on_actionRemove_Selected_triggered()
{
	int index = ui->tableViewTVShowList->currentIndex().row();
	_TVShowModelList->removeSingleRow(index);
}

void MainWindow::on_actionClear_List_triggered()
{
	_TVShowModelList->removeAll();
}

void MainWindow::on_actionExit_triggered()
{
	exit(0);
}

void MainWindow::on_pushButtonSave_clicked()
{

}

void MainWindow::on_pushButtonMove_clicked()
{

}

void MainWindow::on_pushButtonCopy_clicked()
{

}

void MainWindow::on_pushButtonGetTitle_clicked()
{

}

bool MainWindow::ConvertFileName()
{
	QVector<TVShowClass> TVShowModel = _TVShowModelList->getList();

    QRegExp rx("(\\d+)[\\w]+(\\d+)");

    foreach(TVShowClass TVShowInfo, TVShowModel)
    {
        int pos = rx.indexIn(TVShowInfo.FileName());
        QStringList list = rx.capturedTexts();
        if(list.size()>0)
        {
            QMessageBox msgBox;
            msgBox.setText(list[1]+"--"+list[2]);
            msgBox.exec();

        }


    }
	return true;
}

void MainWindow::RecurseDirectory(const QString& sDir)
{
	QDir dir(sDir);
	QFileInfoList list = dir.entryInfoList();
	for (int iList=0;iList<list.count();iList++)
	{
		QFileInfo info = list[iList];

		QString sFilePath = info.filePath();
		if (info.isDir())
		{
			// recursive
			if (info.fileName()!=".." && info.fileName()!=".")
			{
				RecurseDirectory(sFilePath);
			}
		}
		else
		{
			QFileInfo fi(sFilePath);
			if(_UsableEXT.contains(fi.suffix(),Qt::CaseInsensitive))
			{
				QDir fullinfo(sFilePath);
				_TVShowModelList->addTVShowItem(TVShowClass(fullinfo.toNativeSeparators(fi.absoluteDir().path()), fi.fileName(),fi.suffix()));
			}
		}
	}
}

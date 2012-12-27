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
	_UsableEXT<<"avi"<<"mp4"<<"mkv"<<"mov";

    _MainSettings = new SettingsDialog();
    _ConvertionSettings = new ConvertionDialog();

	ui->tableViewTVShowList->setColumnHidden(FILEFOLDER_COLUMN,true);
	//ui->tableViewTVShowList->setColumnHidden(FILEFOLDER_COLUMN,true);
	//ui->tableViewTVShowList->setColumnHidden(FILEFOLDER_COLUMN,true);
	//ui->tableViewTVShowList->setColumnHidden(FILEFOLDER_COLUMN,true);
	//ui->tableViewTVShowList->setColumnHidden(FILEFOLDER_COLUMN,true);
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
		_TVShowModelList->addTVShowItem(TVShowClass(fullinfo.toNativeSeparators(fi.absoluteDir().path()), fi.fileName(),fi.suffix ()));
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

void MainWindow::on_actionConvertion_Profiles_triggered()
{
    _ConvertionSettings->show();
}


void MainWindow::on_actionOptions_triggered()
{
    _MainSettings->show();
}

void MainWindow::on_pushButtonSave_clicked()
{
	int rowCount = _TVShowModelList->rowCount();
	for(int i = 0;i<rowCount;i++)
	{
		TVShowClass TVShowInfoTemp = _TVShowModelList->getData(i);

		if (TVShowInfoTemp.FileName() == TVShowInfoTemp.NewFileName()) continue;

		QFile temp(TVShowInfoTemp.FileFolder()+QDir::separator()+   TVShowInfoTemp.FileName());
		//QFile file(TVShowInfo.FullFileName());
		if(temp.rename(TVShowInfoTemp.FileFolder()+QDir::separator()+ TVShowInfoTemp.NewFileName()))
		{
			TVShowInfoTemp.setFileName(TVShowInfoTemp.NewFileName());
			TVShowInfoTemp.setFileTitle("");
			_TVShowModelList->setData(i,TVShowInfoTemp);
		}
		else
		{
			QMessageBox myBox;
			myBox.setText("Rename Failed:"+TVShowInfoTemp.FileFolder()+"to:"+TVShowInfoTemp.NewFileName());
			myBox.exec();
		}
	}
	ui->tableViewTVShowList->resizeColumnsToContents();
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
	int rowCount = _TVShowModelList->rowCount();
	int columnCount = _TVShowModelList->columnCount();

	QString tempSpace = " ";
	QString notTempSpace = ".";
	QString SeasonDash ="";// "- ";
	QString TitleDash = "- ";


	QRegExp rxFormat("(\\d+)[x|X](\\d+)");
	QRegExp rxSeasonFormat("[s|S](\\d+)[e|E](\\d+)");

	for(int i = 0;i<rowCount;i++)
	{
		TVShowClass TVShowInfo = _TVShowModelList->getData(i);
		QString Extention = TVShowInfo.FileExtention();
		QString ShowTitle = "";
		bool found = false;

		TVShowInfo.setNewFileName(QString(TVShowInfo.FileName()).replace(notTempSpace,tempSpace));

		//1x01 format
		int pos = rxFormat.indexIn(TVShowInfo.NewFileName());
		QStringList List = rxFormat.capturedTexts();

		if(List[1]!="")
		{
			TVShowInfo.setSeasonNum(List[1].toInt());
			TVShowInfo.setEpisodeNum(List[2].toInt());
			TVShowInfo.setTVShowName(TVShowInfo.NewFileName().left(pos-1));
			found = true;
			//QMessageBox myBox;
			//myBox.setText(List[1]+"__"+List[2]+"__"+TVShowInfo.FileName()+"__"+QString::number(pos));
			//myBox.exec();
		}

		//s01e01 format
		pos = rxSeasonFormat.indexIn(TVShowInfo.NewFileName());
		List = rxSeasonFormat.capturedTexts();

		if(List[1]!=""&!found)
		{
			TVShowInfo.setSeasonNum(List[1].toInt());
			TVShowInfo.setEpisodeNum(List[2].toInt());
			TVShowInfo.setTVShowName(TVShowInfo.NewFileName().left(pos-1));
			found = true;
		}

		if(found)
		{
			TVShowInfo.setTVShowName(QString(TVShowInfo.TVShowName()).trimmed());
			QString FormatedSeasonNumber = QString::number(TVShowInfo.SeasonNum());
			QString FormatedEpisodeNumber = QString::number(TVShowInfo.EpisodeNum());
			//check if i is less than 10
			if (TVShowInfo.SeasonNum() < 10)
			FormatedSeasonNumber = "0" +QString::number(TVShowInfo.SeasonNum());
			//check if j is less than 10
			if (TVShowInfo.EpisodeNum() < 10)
			FormatedEpisodeNumber = "0" + QString::number(TVShowInfo.EpisodeNum());

			QString finalShowName = TVShowInfo.TVShowName() + tempSpace + SeasonDash + QString::number(TVShowInfo.SeasonNum()) + "x" + FormatedEpisodeNumber + tempSpace + TitleDash + ShowTitle +"."+ Extention;
			
			finalShowName.replace("..", ".");
			finalShowName.replace(" .", ".");
			finalShowName.replace("- -", "-");
			finalShowName.replace(".-.", ".");
			finalShowName.replace("-.", ".");
			finalShowName.replace(" .", ".");
			
			TVShowInfo.setNewFileName(finalShowName);
		}

		_TVShowModelList->setData(i,TVShowInfo);
	}

	ui->tableViewTVShowList->resizeColumnsToContents();
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

void MainWindow::LoadSettings()
{
    //QSettings settings(QSettings::IniFormat,"ScottNation", "TV Show Renamer");
}

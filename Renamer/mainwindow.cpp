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

	_ConvertionSettingsQSettings = new QSettings(QSettings::IniFormat,QSettings::UserScope,"ScottNation", "TV Show Renamer");
	LoadSettings();

	ui->tableViewTVShowList->setColumnHidden(FILEFOLDER_COLUMN,true);
	//ui->tableViewTVShowList->setColumnHidden(FILEFOLDER_COLUMN,true);
	//ui->tableViewTVShowList->setColumnHidden(FILEFOLDER_COLUMN,true);
	//ui->tableViewTVShowList->setColumnHidden(FILEFOLDER_COLUMN,true);
	//ui->tableViewTVShowList->setColumnHidden(FILEFOLDER_COLUMN,true);
}

MainWindow::~MainWindow()
{
	SaveSettings();
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
	if(_ConvertionSettings->exec())
		ConvertFileName();
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
	//int columnCount = _TVShowModelList->columnCount();

	QString tempSpace = " ";
	QString notTempSpace = ".";
	QString SeasonDash = "";
	QString TitleDash = "";

	if(!_ConvertionSettings->SpaceAsSeporator())
	{
		tempSpace = ".";
		notTempSpace = " ";
	}

	if(_ConvertionSettings->SeasonDash())SeasonDash="-"+tempSpace;
	if(_ConvertionSettings->TitleDash())TitleDash="-"+tempSpace;

	QRegExp rxFormat("(\\d+)[x|X](\\d+)");
	QRegExp rxSeasonFormat("[s|S](\\d+)[e|E](\\d+)");
	QRegExp rxNumberFormat("(\\d+)[.](\\d+)");
	QRegExp rxSpecialFormat("[s|S](\\d+)");

	for(int i = 0;i<rowCount;i++)
	{
		TVShowClass TVShowInfo = _TVShowModelList->getData(i);
		QString Extention = TVShowInfo.FileExtention();
		QString ShowTitle = "";
		bool found = false;

		//replace periods(".") with spaces 
		TVShowInfo.setNewFileName(QString(TVShowInfo.NewFileName()).replace(notTempSpace,tempSpace));

		//Replace "_" with spaces
		if(_ConvertionSettings->ConvertUnderScores())
		{
			TVShowInfo.setNewFileName(QString(TVShowInfo.NewFileName()).replace("_",tempSpace));
		}

		//Replace "-" with spaces
		if(_ConvertionSettings->RemoveDashes())
		{
			TVShowInfo.setNewFileName(QString(TVShowInfo.NewFileName()).replace("-",tempSpace));
		}

		//Replace (), {}, and [] with spaces
		if (_ConvertionSettings->RemoveBrackets())
		{
			TVShowInfo.setNewFileName(QString(TVShowInfo.NewFileName()).replace("(",tempSpace).replace(")",tempSpace)
				.replace("{", tempSpace).replace("}", tempSpace).replace("[", tempSpace).replace("]", tempSpace));
		}

		//Removes extra Spaces and periods
		QStringList tempspace;
		tempspace.append( tempSpace);

		//loop to create arrays of periods/spaces
		for (int i = 1; i < TVShowInfo.NewFileName().size(); i++)
			tempspace.append( tempspace[i - 1] + tempSpace);

		for (int k = TVShowInfo.NewFileName().size() - 1; k > 0; k--)
			TVShowInfo.setNewFileName(QString(TVShowInfo.NewFileName()).replace(tempspace[k],tempSpace));


		//1x01 format
		int pos = rxFormat.indexIn(TVShowInfo.NewFileName());
		QStringList List = rxFormat.capturedTexts();

		if(List[1]!="")
		{
			TVShowInfo.setSeasonNum(List[1].toInt());
			TVShowInfo.setEpisodeNum(List[2].toInt());
			TVShowInfo.setTVShowName(TrimExtraChar(TVShowInfo.NewFileName().left(pos-1)));
			found = true;
			//QMessageBox myBox;
			//myBox.setText(List[1]+"__"+List[2]+"__"+TVShowInfo.FileName()+"__"+QString::number(pos));
			//myBox.exec();
		}
		else
		{
			//s01e01 format
			pos = rxSeasonFormat.indexIn(TVShowInfo.NewFileName());
			List = rxSeasonFormat.capturedTexts();

			if((List[1]!=""))
			{
				TVShowInfo.setSeasonNum(List[1].toInt());
				TVShowInfo.setEpisodeNum(List[2].toInt());
				TVShowInfo.setTVShowName(TrimExtraChar(TVShowInfo.NewFileName().left(pos-1)));
				found = true;
			}
			else
			{
				//01.01 format
				pos = rxNumberFormat.indexIn(TVShowInfo.NewFileName());
				List = rxNumberFormat.capturedTexts();

				if((List[1]!=""))
				{
					TVShowInfo.setSeasonNum(List[1].toInt());
					TVShowInfo.setEpisodeNum(List[2].toInt());
					TVShowInfo.setTVShowName(TrimExtraChar(TVShowInfo.NewFileName().left(pos-1)));
					found = true;
				}
				else
				{
					//S01 format
					pos = rxSpecialFormat.indexIn(TVShowInfo.NewFileName());
					List = rxSpecialFormat.capturedTexts();

					if((List[1]!=""))
					{
						TVShowInfo.setSeasonNum(List[1].toInt());
						//TVShowInfo.setEpisodeNum(List[2].toInt());
						TVShowInfo.setTVShowName(TrimExtraChar(TVShowInfo.NewFileName().left(pos-1)));
						found = true;
					}
				}
			}
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
	_ConvertionSettingsQSettings->beginGroup("ConvertionSettings");
	QHash<QString, QVariant> hash;
	const QStringList keys = _ConvertionSettingsQSettings->allKeys();
	Q_FOREACH(QString key, keys) 
	{
		hash[key] = _ConvertionSettingsQSettings->value(key);
	}
	_ConvertionSettings->SetConvertionSettings(hash);
	_ConvertionSettingsQSettings->endGroup();
}

void MainWindow::SaveSettings()
{
	_ConvertionSettingsQSettings->beginGroup("ConvertionSettings");
	QHash<QString, QVariant> hash = _ConvertionSettings->GetConvertionSettings();

	QHashIterator<QString, QVariant> i(hash);
	while (i.hasNext())
	{
		i.next();
		_ConvertionSettingsQSettings->setValue(i.key(),i.value());
	}
	_ConvertionSettingsQSettings->endGroup();

	_ConvertionSettingsQSettings->sync();
}

QString MainWindow::TrimExtraChar(QString string)
{
	for(int i = string.size()-1;i>=0;i--)
	{
		if(string[i].isLetterOrNumber())
			break;
		else
			string.remove(i,1);
	}

	for(int j = 0;j<string.size();j++)
	{
		if(string[0].isLetterOrNumber())
			break;
		else
			string.remove(0,1);
	}

	return string;
}

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
	for (int index = 0; index < TVShowModel.size(); index++)
            {
                bool addSeasonFormat = true;

                if (!TVShowModel[index].Auto())
                    continue;
                QString newfilename = TVShowModel[index].FileName();
                QString extend = TVShowModel[index].FileExtention();
                QString showTitle = TVShowModel[index].FileTitle();
                QString tvshowName = "";
                QString finalShowName = "";
                QString temp = "";
                QString seasonDash = "";
                QString titleDash = "";
                QString formattedSeason = "";
                QString formattedEpisode = "";
                int startIndex = -1;
                int endIndex = -1;

                TVShowModel[index].setGetTitle( true);

                if (!newMainSettings.DashSeason)
                    seasonDash = "- ";
                if (!newMainSettings.DashTitle)
                    titleDash = "- ";
                if (newMainSettings.RemovePeriod)
                    temp = " ";
                else
                    temp = ".";

                //remove extention
                newfilename = newfilename.replace(extend, temp + "&&&&");

                //add word at begining
                if (newMainSettings.FirstWord != "")
                {
                    newfilename = newMainSettings.FirstWord + temp + newfilename;
                    //newMainSettings.FirstWord = "";
                }

                //Text converter            
                textConverter = textConvert.getText();
                for (int x = 0; x < textConverter.Count(); x += 2)
                    newfilename = newfilename.replace(textConverter[x], textConverter[x + 1]);

                //user junk list
                if (newMainSettings.RemoveCrap)
                {
                    //make user junk list
                    userjunklist = userJunk.getjunk();
                    if (userjunklist.Count() != 0)
                    {
                        for (int x = 0; x < userjunklist.Count(); x++)
                            newfilename = newfilename.replace(userjunklist[x], "");
                    }//end of if
                }//end of removeExtraCrapToolStripMenuItem if

                //replace periods(".") with spaces 
                if (newMainSettings.RemovePeriod)
                    newfilename = newfilename.replace(".", temp);

                //replace "_" with spaces
                if (newMainSettings.RemoveUnderscore)
                    newfilename = newfilename.replace("_", temp);

                //replace "-" with spaces
                if (newMainSettings.RemoveDash)
                    newfilename = newfilename.replace("-", temp);

                //replace (), {}, and [] with spaces
                if (newMainSettings.RemoveBracket)
                    newfilename = newfilename.replace("(", temp).replace(")", temp).replace("{", temp).replace("}", temp).replace("[", temp).replace("]", temp);

                //make every thing lowercase for crap remover to work
                newfilename = newfilename.toLower();// s.ToString();

                //remove extra crap 
                if (newMainSettings.RemoveCrap)
                {
                    //new way with file input
                    for (int x = 0; x < junklist.size(); x++)
                        newfilename = newfilename.replace(junklist[x] + temp, temp);
                }//end of removeExtraCrapToolStripMenuItem if

                //remove begining space
                newfilename = newfilename.TrimStart(temp.ToCharArray());

                //remove year function
                if (newMainSettings.RemoveYear && (!(newMainSettings.SeasonFormat == 4)))
                {
                    int curyear = System.DateTime.Now.Year;
                    for (; curyear > 1900; curyear--)
                    {
                        QString before = newfilename;
                        newfilename = newfilename.replace(curyear.ToString(), "");
                        //break if value found
                        if (before != newfilename)
                            break;
                    }//end of for loop
                }//end of remove year function

                //Removes extra Spaces and periods
                QString tempspace[newfilename.size()];
                QString tempper[newfilename.size()];
                tempspace[0] = " ";
                tempper[0] = ".";

                //loop to create arrays of periods/spaces
                for (int i = 1; i < newfilename.size(); i++)
                {
                    tempspace[i] = tempspace[i - 1] + " ";
                    tempper[i] = tempper[i - 1] + ".";
                }//end of for 

                for (int k = newfilename.size() - 1; k > 0; k--)
                {
                    newfilename = newfilename.replace(tempspace[k], " ");
                    newfilename = newfilename.replace(tempper[k], ".");
                }//end of for                          

                //loop for seasons
                for (int i = 1; i < 41; i++)
                {
                    //varable for break command later
                    bool end = false;

                    if (i == 40) i = 0;

                    //loop for episodes
                    for (int j = 0; j < 150; j++)
                    {
                        QString newi = i.ToString();
                        QString newi2 = (i + newMainSettings.SeasonOffset).ToString();
                        QString newj = j.ToString();

                        QString newj2 = (j + newMainSettings.EpisodeOffset).ToString();
                        QString output = '';

                        //check if i is less than 10
                        if (i < 10)
                            newi = "0" + i.ToString();
                        //check if j is less than 10
                        if (j < 10)
                            newj = "0" + j.ToString();
                        if ((i + newMainSettings.SeasonOffset) < 10)
                            newi2 = "0" + (i + newMainSettings.SeasonOffset).ToString();
                        //check if j is less than 10
                        if ((j + newMainSettings.EpisodeOffset) < 10)
                            newj2 = "0" + (j + newMainSettings.EpisodeOffset).ToString();

                        //1x01 format 
                        if (newMainSettings.SeasonFormat == 0)
                        {
                            output = i.ToString() + "x" + newj;

                            newfilename = newfilename.replace(temp + i.ToString() + newj + temp, temp + output + temp);//101
                            newfilename = newfilename.replace(temp + newi + newj + temp, temp + output + temp);//0101                        
                            newfilename = newfilename.replace("s" + i.ToString() + "e" + j.ToString() + temp, output + temp);//s1e1               
                            newfilename = newfilename.replace("s" + i.ToString() + "e" + newj + temp, output + temp);//s1e01
                            newfilename = newfilename.replace("s" + newi + "e" + newj + temp, output + temp);//s01e01
                            newfilename = newfilename.replace("s" + newi + " e" + newj + temp, output + temp);//s01 e01
                            newfilename = newfilename.replace("season " + i.ToString() + " episode " + newj + temp, output + temp);//season 1 episode 01
                            newfilename = newfilename.replace("season " + i.ToString() + " episode " + j.ToString() + temp, output + temp);//season 1 episode 1
                            newfilename = newfilename.replace(temp + i.ToString() + temp + newj + temp, temp + output + temp);//1 01

                            startIndex = newfilename.IndexOf(temp + output + temp);//find index                        
                            if (startIndex != -1)
                            {
                                if (i > 9) { endIndex = startIndex + 6; } else { endIndex = startIndex + 5; }
                                //endIndex = startIndex + 4;
                            }
                        }

                        //0101 format
                        if (newMainSettings.SeasonFormat == 1)
                        {
                            output = newi + newj;

                            newfilename = newfilename.replace(temp + i.ToString() + newj + temp, temp + output + temp);//101 
                            newfilename = newfilename.replace(temp + i.ToString() + "x" + newj + temp, temp + output + temp);//1x01
                            newfilename = newfilename.replace("s" + newi + "e" + newj + temp, output + temp);//s01e01
                            newfilename = newfilename.replace("s" + newi + " e" + newj + temp, output + temp);//s01 e01
                            newfilename = newfilename.replace("s" + i.ToString() + "e" + j.ToString() + temp, output + temp);//s1e1
                            newfilename = newfilename.replace("s" + i.ToString() + "e" + newj + temp, output + temp);//s1e01
                            newfilename = newfilename.replace("season " + i.ToString() + " episode " + newj + temp, output + temp);//season 1 episode 01
                            newfilename = newfilename.replace("season " + i.ToString() + " episode " + j.ToString() + temp, output + temp);//season 1 episode 1
                            newfilename = newfilename.replace(temp + i.ToString() + temp + newj + temp, temp + output + temp);// 1 01 

                            startIndex = newfilename.IndexOf(temp + output + temp);//find index
                            if (startIndex != -1)
                                endIndex = startIndex + 5;
                        }

                        //S01E01 format
                        if (newMainSettings.SeasonFormat == 2)
                        {
                            output = "S" + newi + "E" + newj;

                            newfilename = newfilename.replace(temp + i.ToString() + newj + temp, temp + output + temp);//101
                            newfilename = newfilename.replace(temp + i.ToString() + "x" + newj + temp, temp + output + temp);//1x01
                            newfilename = newfilename.replace(newi + newj + temp, output + temp);//0101
                            newfilename = newfilename.replace("s" + newi + "e" + newj + temp, output + temp);//s01E01
                            newfilename = newfilename.replace("s" + i.ToString() + "e" + j.ToString() + temp, output + temp);//s1e1
                            newfilename = newfilename.replace("s" + i.ToString() + "e" + newj + temp, output + temp);//s1e01
                            newfilename = newfilename.replace("s" + newi + " e" + newj + temp, output + temp);//s01 e01
                            newfilename = newfilename.replace("season " + i.ToString() + " episode " + newj + temp, output + temp);//season 1 episode 01
                            newfilename = newfilename.replace("season " + i.ToString() + " episode " + j.ToString() + temp, output + temp);//Season 1 Episode 1
                            newfilename = newfilename.replace(temp + i.ToString() + temp + newj + temp, temp + output + temp);//1 01

                            startIndex = newfilename.IndexOf(temp + output + temp);//find index
                            if (startIndex != -1)
                                endIndex = startIndex + 7;
                        }

                        //101 format
                        if (newMainSettings.SeasonFormat == 3)
                        {
                            output = i.ToString() + newj;

                            newfilename = newfilename.replace(temp + newi + newj + temp, temp + output + temp);//0101
                            newfilename = newfilename.replace(temp + i.ToString() + "x" + newj + temp, temp + output + temp);//1x01
                            newfilename = newfilename.replace("s" + newi + "e" + newj + temp, output + temp);//s01e01
                            newfilename = newfilename.replace("s" + newi + " e" + newj + temp, output + temp);//s01 e01
                            newfilename = newfilename.replace("s" + i.ToString() + "e" + j.ToString() + temp, output + temp);//s1e1
                            newfilename = newfilename.replace("s" + i.ToString() + "e" + newj + temp, output + temp);//s1e01
                            newfilename = newfilename.replace("season " + i.ToString() + " episode " + newj + temp, output + temp);//season 1 episode 01
                            newfilename = newfilename.replace("season " + i.ToString() + " episode " + j.ToString() + temp, output + temp);//season 1 episode 1
                            newfilename = newfilename.replace(temp + i.ToString() + temp + newj + temp, temp + output + temp);// 1 01 

                            startIndex = newfilename.IndexOf(temp + output + temp);//find index
                            if (startIndex != -1)
                            {
                                if (i > 9) { endIndex = startIndex + 5; } else { endIndex = startIndex + 4; }
                                //endIndex = startIndex + 3;
                            }
                        }

                        //stop loop when name is change                    
                        if (startIndex != -1)
                        {
                            EditFileList[index].SeasonNum = i + newMainSettings.SeasonOffset;
                            formattedSeason = newi2;
                            EditFileList[index].EpisodeNum = j + newMainSettings.EpisodeOffset;
                            formattedEpisode = newj2;
                            end = true;
                            break;
                        }

                    }//end of episode loop

                    //stop loop when name is change
                    if (end)
                        break;
                    if (i == 0) i = 40;
                }//end of season loop 

                //Date format
                if (newMainSettings.SeasonFormat == 4)
                {
                    for (int year = 0; year < 20; year++)
                    {
                        bool end = false;
                        for (int month = 12; month > 0; month--)
                        {
                            for (int day = 31; day > 0; day--)
                            {
                                //QString startnewname = newfilename;
                                QString newyear = year.ToString();
                                QString newmonth = month.ToString();
                                QString newday = day.ToString();

                                //check if i is less than 10
                                if (year < 10)
                                    newyear = "0" + year.ToString();
                                //check if j is less than 10
                                if (month < 10)
                                    newmonth = "0" + month.ToString();
                                //check if k is less than 10
                                if (day < 10)
                                    newday = "0" + day.ToString();
                                QString kk = "20" + newyear;

                                newfilename = newfilename.replace(kk + " " + month + " " + day, month.ToString() + "-" + day.ToString() + "-" + kk);
                                newfilename = newfilename.replace(kk + " " + newmonth + " " + newday, month.ToString() + "-" + day.ToString() + "-" + kk);

                                startIndex = newfilename.IndexOf(month + "-" + day + "-" + kk);//find index
                                if (startIndex != -1)
                                {
                                    if (day > 9 && month > 9)
                                        endIndex = startIndex + 10;
                                    else if (day > 9 || month > 9)
                                        endIndex = startIndex + 9;
                                    else
                                        endIndex = startIndex + 8;

                                    EditFileList[index].SeasonNum = (month * 100) + day;
                                    formattedSeason = month.ToString() + "-" + day.ToString();
                                    formattedEpisode = kk;
                                    EditFileList[index].EpisodeNum = Int32.Parse(kk);
                                    end = true;
                                    break;
                                }
                            }//end of for loop day
                            if (end)
                                break;
                        }//end of for loop month
                        if (end)
                            break;
                    }//end of for loop year
                }//end of if for date check box 

                if (startIndex == -1)
                {
                    startIndex = newfilename.size() - 5;
                    EditFileList[index].GetTitle = false;
                    addSeasonFormat = false;
                }

                tvshowName = newfilename.Substring(0, startIndex).Trim();

                EditFileList[index].TVShowID = SearchTVShowName(tvshowName);

                bool useOldTiltle = true;
                if (newMainSettings.GetTVShowName)
                {
                    if (EditFileList[index].TVShowID != -1)
                    {
                        QString newTvshowName = TVShowInfoList[EditFileList[index].TVShowID].RealTVShowName;
                        if (newTvshowName != "")
                        {
                            tvshowName = newTvshowName;
                            useOldTiltle = false;
                        }
                    }
                }

                if (useOldTiltle)
                {
                    switch (newMainSettings.ProgramFormat)
                    {
                        case 0:
                            tvshowName = UpperCaseingAfterSpace(tvshowName, 0, tvshowName.Length - 1);
                            break;
                        case 1:
                            tvshowName = UpperCaseing(tvshowName, 0, 1);
                            break;
                        case 2:
                            tvshowName = UpperCaseing(tvshowName, 0, tvshowName.Length);
                            break;
                        default:
                            break;
                    }
                }

                if (newMainSettings.TitleFormat != 5 && EditFileList[index].GetTitle)
                {
                    switch (newMainSettings.TitleSelection)
                    {
                        case 0:
                            if (!secondTime)
                            {
                                showTitle = "";
                                if (endIndex != newfilename.size() - 5)
                                    showTitle = newfilename.Substring(endIndex, newfilename.size() - (endIndex + 5)).Trim();
                                if (showTitle != "")
                                    EditFileList[index].GetTitle = false;
                            }
                            break;
                        case 1:
                            EditFileList[index].GetTitle = false;
                            showTitle = "";
                            if (endIndex != newfilename.size() - 5)
                                showTitle = newfilename.Substring(endIndex, newfilename.size() - (endIndex + 5)).Trim();
                            break;
                        case 2:

                            break;
                        default:
                            break;
                    }

                    switch (newMainSettings.TitleFormat)
                    {
                        case 0:
                            showTitle = UpperCaseingAfterSpace(showTitle, 0, showTitle.size() - 1);
                            break;
                        case 1:
                            showTitle = showTitle.toLower();// = lowering(showTitle);
                            showTitle = UpperCaseingAfterSpace(showTitle, 0, showTitle.size() - 1);
                            break;
                        case 2:
                            showTitle = showTitle.toLower();
                            showTitle = UpperCaseing(showTitle, 0, 1);
                            break;
                        case 3:
                            showTitle = UpperCaseing(showTitle, 0, showTitle.size());
                            break;
                        case 4:
                            showTitle = showTitle.toLower();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    EditFileList[index].FileTitle = showTitle = "";
                    EditFileList[index].GetTitle = false;
                }

                switch (newMainSettings.ExtFormat)
                {
                    case 0:
                        extend = extend.toLower();
                        break;
                    case 1:
                        StringBuilder ext1 = new StringBuilder(extend);
                        ext1[1] = char.ToUpper(ext1[1]);
                        extend = ext1.ToString();
                        break;
                    case 2:
                        extend = extend.toUpper();
                        break;
                    default:
                        break;
                }

                tvshowName = tvshowName.replace("Vs", "vs");
                tvshowName = tvshowName.replace("O C ", "O.C. ");
                tvshowName = tvshowName.replace("T O ", "T.O. ");
                tvshowName = tvshowName.replace("Csi", "CSI");
                tvshowName = tvshowName.replace("Wwii", "WWII");
                tvshowName = tvshowName.replace("Hd", "HD");
                tvshowName = tvshowName.replace("Tosh 0", "Tosh.0");
                tvshowName = tvshowName.replace("O Brien", "O'Brien");
                tvshowName = tvshowName.replace("Nbc", "NBC");
                tvshowName = tvshowName.replace("Abc", "ABC");
                tvshowName = tvshowName.replace("Cbs", "CBS");
                tvshowName = tvshowName.replace("Iv" + temp, "IV" + temp);
                tvshowName = tvshowName.replace("Ix" + temp, "IX" + temp);
                tvshowName = tvshowName.replace("Viii", "VIII");
                tvshowName = tvshowName.replace("Vii", "VII");
                tvshowName = tvshowName.replace("Vi" + temp, "VI" + temp);
                tvshowName = tvshowName.replace("Xi" + temp, "XI" + temp);
                tvshowName = tvshowName.replace("Xii" + temp, "XII" + temp);
                tvshowName = tvshowName.replace("Xiii" + temp, "XIII" + temp);
                tvshowName = tvshowName.replace("Xiiii" + temp, "XIIII" + temp);
                tvshowName = tvshowName.replace("Iii", "III");
                tvshowName = tvshowName.replace("Ii", "II");
                tvshowName = tvshowName.replace("X Files", "X-Files");
                tvshowName = tvshowName.replace("La ", "LA ");
                tvshowName = tvshowName.replace("Nba", "NBA");
                tvshowName = tvshowName.replace("Espn", "ESPN");

                if (addSeasonFormat && newMainSettings.SeasonFormat != 5)
                {
                    //add file extention back on 
                    switch (newMainSettings.SeasonFormat)
                    {
                        case 0:
                            finalShowName = tvshowName + temp + seasonDash + EditFileList[index].SeasonNum.ToString() + "x" + formattedEpisode + temp + titleDash + showTitle + extend;
                            break;
                        case 1:
                            finalShowName = tvshowName + temp + seasonDash + formattedSeason + formattedEpisode + temp + titleDash + showTitle + extend;
                            break;
                        case 2:
                            finalShowName = tvshowName + temp + seasonDash + "S" + formattedSeason + "E" + formattedEpisode + temp + titleDash + showTitle + extend;
                            break;
                        case 3:
                            finalShowName = tvshowName + temp + seasonDash + EditFileList[index].SeasonNum.ToString() + formattedEpisode + temp + titleDash + showTitle + extend;
                            break;
                        case 4:
                            finalShowName = tvshowName + temp + seasonDash + formattedSeason + "-" + formattedEpisode + temp + titleDash + showTitle + extend;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    finalShowName = tvshowName + extend;
                }
                //newfilename = newfilename.replace(temp + "&&&&", extend);

                //Random fixes
                finalShowName = finalShowName.replace("..", ".");
                finalShowName = finalShowName.replace(" .", ".");
                finalShowName = finalShowName.replace("- -", "-");
                finalShowName = finalShowName.replace(".-.", ".");
                finalShowName = finalShowName.replace("-.", ".");
                finalShowName = finalShowName.replace(" .", ".");

                //finalShowName = finalShowName.replace("Web Dl", "WEB-DL");

                EditFileList[index].FileTitle = showTitle;
                EditFileList[index].TVShowName = tvshowName;
                EditFileList[index].NewFileName = finalShowName;
            }
            // return converted file name
            return true;
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

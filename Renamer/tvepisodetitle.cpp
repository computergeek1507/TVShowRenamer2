#include "tvepisodetitle.h"

tvepisodetitle::tvepisodetitle()
{
	//QUrl url("http://www.thetvdb.com/api/GetSeries.php");
}

int tvepisodetitle::getTVDBID(QString ShowName,int index)
{
	QString destination_XML_URL = QString("http://www.thetvdb.com/api/GetSeries.php?seriesname=%1&language=en").arg(ShowName);
	QUrl newURL = QUrl(destination_XML_URL,QUrl::TolerantMode);
	manager = new QNetworkAccessManager(this);
	
	connect(manager, SIGNAL(finished(QNetworkReply*)),
		this, SLOT(replyFinished(QNetworkReply*)));
	manager->get(QNetworkRequest(newURL));
	return -1;
}

void tvepisodetitle::replyFinished(QNetworkReply * reply)
{
	QString data=(QString)reply->readAll();
	QXmlStreamReader xml(data);
	//QXmlStreamAttributes attrib;
	QString CurrentTagName;
	QString SeriesName;
	int SeriesID;
	QMap<QString, int> ShowNameList;
	while(!xml.atEnd())
	{
		xml.readNext();
		if (xml.isStartElement())
			CurrentTagName = xml.name().toString();
		else if (xml.isEndElement())
		{
			if (xml.name()== "Series")
			{
				ShowNameList[SeriesName] = SeriesID;
				SeriesName.clear();
				SeriesID = -1;
			}
			CurrentTagName = "";
		}
		else if (xml.isCharacters() && !xml.isWhitespace())
		{
			if (CurrentTagName == "seriesid")
			{
				SeriesID = xml.text().toString().toInt();
			}
			else if (CurrentTagName == "SeriesName")
			{
				SeriesName = xml.text().toString();
			}
		}
	}

	//QMessageBox myBox;
	//myBox.setText(name);
	//myBox.exec();
}
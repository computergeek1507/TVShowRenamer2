#ifndef TVEPISODETITLE_H
#define TVEPISODETITLE_H

#include <QUrl>
#include <QNetworkRequest>
#include <QNetworkAccessManager>
#include <QNetworkReply>
#include <QObject>
#include <QXmlStreamReader>
#include <QDebug>
#include <QMessageBox>

class tvepisodetitle : public QObject
{
	Q_OBJECT
	//http://www.thetvdb.com/api/GetSeries.php?seriesname=house&language=en
public:
	tvepisodetitle();
	int getTVDBID(QString ShowName,int index);

public slots:
	void replyFinished(QNetworkReply * reply);

private:
	QNetworkAccessManager *manager;
	
	
};

#endif // TVEPISODETITLE_H

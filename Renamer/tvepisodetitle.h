#ifndef TVEPISODETITLE_H
#define TVEPISODETITLE_H

#include <QUrl>
#include <QtXml/QDomDocument>
#include <QtXml/QDomNodeList>
#include <QtWebKit/QWebView>

class tvepisodetitle
{
	//http://www.thetvdb.com/api/GetSeries.php?seriesname=house&language=en
public:
    tvepisodetitle();

	public slots:
	void replyFinished(QNetworkReply*);

	private:
    QNetworkAccessManager *manager;
    QNetworkReply * reply;
};

#endif // TVEPISODETITLE_H

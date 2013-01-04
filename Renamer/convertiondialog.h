#ifndef CONVERTIONDIALOG_H
#define CONVERTIONDIALOG_H

#include <QDialog>
#include <QMap>
#include <QSettings>
#include <QHash>

namespace Ui
{
	class ConvertionDialog;
}

class ConvertionDialog : public QDialog
{
	Q_OBJECT

public:
	explicit ConvertionDialog(QWidget *parent = 0);
	~ConvertionDialog();

	QHash<QString, QVariant> GetConvertionSettings();
	void SetConvertionSettings(QHash<QString, QVariant> ConvertionSettings);

	const bool& SeasonDash      () const;
	const bool& TitleDash       () const;

	const bool& SpaceAsSeporator   () const;
	const bool& ConvertUnderScores () const;
	const bool& RemoveBrackets     () const;
	const bool& RemoveDashes       () const;
	const bool& RemoveYear         () const;
	const bool& RemoveJunk         () const;
	const bool& AutoGetTitle       () const;
	const bool& UseOnlineShowName  () const;

	const int& TVShowFormat     () const ;//{ return ui->ShowNameComboBox->currentIndex();     }
	const int& SeasonFormat     () const ;
	const int& TitleFormat      () const ;
	const int& ExtFormat        () const ;
	const int& TitleGetSetting  () const ;
	const int& TitleGetLocation () const ;

	const int& SeasonOffset  () const ;
	const int& EpisodeOffset () const ;

	void setSeasonDash       ( const bool& SeasonDash       );
	void setTitleDash        ( const bool& TitleDash        );

	void setSpaceAsSeporator   ( const bool& SpaceAsSeporator   );
	void setConvertUnderScores ( const bool& ConvertUnderScores );
	void setRemoveBrackets     ( const bool& RemoveBrackets     );
	void setRemoveDashes       ( const bool& RemoveDashes       );
	void setRemoveYear         ( const bool& RemoveYear         );
	void setRemoveJunk         ( const bool& RemoveJunk         );
	void setAutoGetTitle       ( const bool& AutoGetTitle       );
	void setUseOnlineShowName  ( const bool& UseOnlineShowName  );

	void setTVShowFormat     ( const int&  TVShowFormat     );//{ ui->ShowNameComboBox->setCurrentIndex( TVShowFormat);    }
	void setSeasonFormat     ( const int&  SeasonFormat     );
	void setTitleFormat      ( const int&  TitleFormat      );
	void setExtFormat        ( const int&  ExtFormat        );
	void setTitleGetSetting  ( const int&  TitleGetSetting  );
	void setTitleGetLocation ( const int&  TitleGetLocation );

	void setSeasonOffset  ( const int&  SeasonOffset  );
	void setEpisodeOffset ( const int&  EpisodeOffset );

private slots:
	void on_OKPushButton_clicked();
	void on_CancelPushButton_clicked();

private:
	Ui::ConvertionDialog *ui;
};

#endif // CONVERTIONDIALOG_H

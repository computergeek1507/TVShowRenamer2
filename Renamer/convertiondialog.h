#ifndef CONVERTIONDIALOG_H
#define CONVERTIONDIALOG_H

#include <QDialog>
#include <QMap>

namespace Ui {
class ConvertionDialog;
}

class ConvertionDialog : public QDialog
{
	Q_OBJECT

public:
	explicit ConvertionDialog(QWidget *parent = 0);
	~ConvertionDialog();

	const bool& SeasonDash      () const;// { return _SeasonDash;       }
	const bool& TitleDash       () const;// { return _TitleDash;        }

	const bool& SpaceAsSeporator   () const;
	const bool& ConvertUnderScores () const;
	const bool& RemoveBrackets     () const;
	const bool& RemoveDashes       () const;
	const bool& RemoveYear         () const;
	const bool& RemoveJunk         () const;
	const bool& AutoGetTitle       () const;
	const bool& UseOnlineShowName  () const;

	const int& TVShowFormat     () const ;//{ return ui->ShowNameComboBox->currentIndex();     }
	const int& SeasonFormat     () const ;//{ return _SeasonFormat;     }
	const int& TitleFormat      () const ;//{ return _TitleFormat;      }
	const int& ExtFormat        () const ;//{ return _ExtFormat;        }
	const int& TitleGetSetting  () const ;//{ return _TitleGetSetting;  }
	const int& TitleGetLocation () const ;//{ return _TitleGetLocation; }

	const int& SeasonOffset  () const ;//{ return _TitleGetSetting;  }
	const int& EpisodeOffset () const ;//{ return _TitleGetLocation; }

	void setSeasonDash       ( const bool& SeasonDash       );//{ _SeasonDash       = SeasonDash;       }
	void setTitleDash        ( const bool& TitleDash        );//{ _TitleDash        = TitleDash;        }

	void setSpaceAsSeporator   ( const bool& SpaceAsSeporator   );
	void setConvertUnderScores ( const bool& ConvertUnderScores );
	void setRemoveBrackets     ( const bool& RemoveBrackets     );
	void setRemoveDashes       ( const bool& RemoveDashes       );
	void setRemoveYear         ( const bool& RemoveYear         );
	void setRemoveJunk         ( const bool& RemoveJunk         );
	void setAutoGetTitle       ( const bool& AutoGetTitle       );
	void setUseOnlineShowName  ( const bool& UseOnlineShowName  );

	void setTVShowFormat     ( const int&  TVShowFormat     );//{ ui->ShowNameComboBox->setCurrentIndex( TVShowFormat);    }
	void setSeasonFormat     ( const int&  SeasonFormat     );//{ _SeasonFormat     = SeasonFormat;     }
	void setTitleFormat      ( const int&  TitleFormat      );//{ _TitleFormat      = TitleFormat;      }
	void setExtFormat        ( const int&  ExtFormat        );//{ _ExtFormat        = ExtFormat;        }
	void setTitleGetSetting  ( const int&  TitleGetSetting  );//{ _TitleGetSetting  = TitleGetSetting;  }
	void setTitleGetLocation ( const int&  TitleGetLocation );//{ _TitleGetLocation = TitleGetLocation; }

	void setSeasonOffset  ( const int&  SeasonOffset  );//{ _TitleGetSetting  = TitleGetSetting;  }
	void setEpisodeOffset ( const int&  EpisodeOffset );//{ _TitleGetLocation = TitleGetLocation; }

private slots:
	void on_OKPushButton_clicked();
	void on_CancelPushButton_clicked();

private:
	Ui::ConvertionDialog *ui;

	//int _TVShowFormat;
	//int _SeasonFormat;
	//int _TitleFormat;
	//int _ExtFormat;
	//int _TitleGetSetting;
	//int _TitleGetLocation;

	int _SeasonOffset;
	int _EpisodeOffset;

	//bool _SeasonDash;
	//bool _TitleDash;
	bool _SpaceAsSeporator;
	bool _ConvertUnderScores;
	bool _RemoveBrackets;
	bool _RemoveDashes;
	bool _RemoveYear;
	bool _RemoveJunk;
	bool _AutoGetTitle;
	bool _UseOnlineShowName;
};

#endif // CONVERTIONDIALOG_H

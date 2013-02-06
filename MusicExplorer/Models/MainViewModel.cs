﻿/*
 * Copyright © 2013 Nokia Corporation. All rights reserved.
 * Nokia and Nokia Connecting People are registered trademarks of Nokia Corporation. 
 * Other product and company names mentioned herein may be trademarks
 * or trade names of their respective owners. 
 * See LICENSE.TXT for license information.
 */

using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MusicExplorer.Models
{
    /// <summary>
    /// MainViewModel - acts as a link between UI and data in models.
    /// A single view model is used to link all the data models to UI.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        // Members
        public ObservableCollection<ArtistModel> LocalAudio { get; private set; }
        public ObservableCollection<ArtistModel> Recommendations { get; private set; }
        public ObservableCollection<ArtistModel> TopArtists { get; private set; }
        public ObservableCollection<ProductModel> TracksForArtist { get; private set; }
        public ObservableCollection<ProductModel> AlbumsForArtist { get; private set; }
        public ObservableCollection<ProductModel> SinglesForArtist { get; private set; }
        public ObservableCollection<ArtistModel> SimilarForArtist { get; private set; }
        public ObservableCollection<ProductModel> NewReleases { get; private set; }
        public ObservableCollection<GenreModel> Genres { get; private set; }
        public ObservableCollection<ArtistModel> TopArtistsForGenre { get; private set; }
        public ObservableCollection<MixGroupModel> MixGroups { get; private set; }
        public ObservableCollection<MixModel> Mixes { get; private set; }

        MediaLibrary mediaLib = null; // For accessing local artists and songs.

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainViewModel()
        {
            LocalAudio = new ObservableCollection<ArtistModel>();
            Recommendations = new ObservableCollection<ArtistModel>();
            NewReleases = new ObservableCollection<ProductModel>();
            TopArtists = new ObservableCollection<ArtistModel>();
            TracksForArtist = new ObservableCollection<ProductModel>();
            AlbumsForArtist = new ObservableCollection<ProductModel>();
            SinglesForArtist = new ObservableCollection<ProductModel>();
            SimilarForArtist = new ObservableCollection<ArtistModel>();
            Genres = new ObservableCollection<GenreModel>();
            TopArtistsForGenre = new ObservableCollection<ArtistModel>();
            MixGroups = new ObservableCollection<MixGroupModel>();
            Mixes = new ObservableCollection<MixModel>();

            SelectedArtist = null;
            SelectedGenre = "";
            SelectedMixGroup = "";

            // Insert a place holder for title text
            LocalAudio.Add(new ArtistModel() { 
                Name = "MusicExplorerTitlePlaceholder", 
                ItemHeight = "110", 
                ItemWidth = "400" 
            });

            // Enable flipping of favourites items after launch
            FlipFavourites = true;
        }

        private ArtistModel _selectedArtist;
        /// <summary>
        /// SelectedArtist property to keep track of user selected artist.
        /// </summary>
        public ArtistModel SelectedArtist
        {
            get
            {
                return _selectedArtist;
            }
            set
            {
                if (value != _selectedArtist)
                {
                    _selectedArtist = value;
                    NotifyPropertyChanged("SelectedArtist");
                }
            }
        }

        private string _selectedGenre;
        /// <summary>
        /// SelectedGenre property.
        /// This property is used in the UI to display its value using a Binding.
        /// </summary>
        public string SelectedGenre
        {
            get
            {
                return _selectedGenre;
            }
            set
            {
                if (value != _selectedGenre)
                {
                    _selectedGenre = value;
                    NotifyPropertyChanged("SelectedGenre");
                }
            }
        }

        private string _selectedMixGroup;
        /// <summary>
        /// SelectedMixGroup property.
        /// This property is used in the UI to display its value using a Binding.
        /// </summary>
        public string SelectedMixGroup
        {
            get
            {
                return _selectedMixGroup;
            }
            set
            {
                if (value != _selectedMixGroup)
                {
                    _selectedMixGroup = value;
                    NotifyPropertyChanged("SelectedMixGroup");
                }
            }
        }

        private string _progressIndicatorText;
        /// <summary>
        /// ProgressIndicatorText property.
        /// This property is used in the UI to display its value using a Binding.
        /// </summary>
        public string ProgressIndicatorText
        {
            get
            {
                return _progressIndicatorText;
            }
            set
            {
                if (value != _progressIndicatorText)
                {
                    _progressIndicatorText = value;
                    NotifyPropertyChanged("ProgressIndicatorText");
                }
            }
        }

        private bool _progressIndicatorVisible;
        /// <summary>
        /// ProgressIndicatorVisible property.
        /// This property is used in the UI to display/hide progress indicator using a Binding.
        /// </summary>
        public bool ProgressIndicatorVisible
        {
            get
            {
                return _progressIndicatorVisible;
            }
            set
            {
                if (value != _progressIndicatorVisible)
                {
                    _progressIndicatorVisible = value;
                    NotifyPropertyChanged("ProgressIndicatorVisible");
                }
            }
        }

        private bool _flipFavourites;
        /// <summary>
        /// FlipFavourites property.
        /// This property is used in the UI to control flipping of favourites items using a Binding.
        /// </summary>
        public bool FlipFavourites
        {
            get
            {
                return _flipFavourites;
            }
            set
            {
                if (value != _flipFavourites)
                {
                    _flipFavourites = value;
                    NotifyPropertyChanged("FlipFavourites");
                }
            }
        }

        private Visibility _noFavouritesVisibility = Visibility.Collapsed;
        /// <summary>
        /// NoFavouritesVisibility property.
        /// This property is used in the UI to show "no favourites available" if necessary.
        /// </summary>
        public Visibility NoFavouritesVisibility
        {
            get
            {
                return _noFavouritesVisibility;
            }
            set
            {
                if (value != _noFavouritesVisibility)
                {
                    _noFavouritesVisibility = value;
                    NotifyPropertyChanged("NoFavouritesVisibility");
                }
            }
        }

        private Visibility _noRecommendedVisibility = Visibility.Collapsed;
        /// <summary>
        /// NoRecommendedVisibility property.
        /// This property is used in the UI to show "no recommendations available" if necessary.
        /// </summary>
        public Visibility NoRecommendedVisibility
        {
            get
            {
                return _noRecommendedVisibility;
            }
            set
            {
                if (value != _noRecommendedVisibility)
                {
                    _noRecommendedVisibility = value;
                    NotifyPropertyChanged("NoRecommendedVisibility");
                }
            }
        }

        private Visibility _noTracksVisibility;
        /// <summary>
        /// NoTracksVisibility property.
        /// This property is used in the UI to show "no tracks available" if necessary.
        /// </summary>
        public Visibility NoTracksVisibility
        {
            get
            {
                return _noTracksVisibility;
            }
            set
            {
                if (value != _noTracksVisibility)
                {
                    _noTracksVisibility = value;
                    NotifyPropertyChanged("NoTracksVisibility");
                }
            }
        }

        private Visibility _noAlbumsVisibility;
        /// <summary>
        /// NoAlbumsVisibility property.
        /// This property is used in the UI to show "no albums available" if necessary.
        /// </summary>
        public Visibility NoAlbumsVisibility
        {
            get
            {
                return _noAlbumsVisibility;
            }
            set
            {
                if (value != _noAlbumsVisibility)
                {
                    _noAlbumsVisibility = value;
                    NotifyPropertyChanged("NoAlbumsVisibility");
                }
            }
        }

        private Visibility _noSinglesVisibility;
        /// <summary>
        /// NoSinglesVisibility property.
        /// This property is used in the UI to show "no singles available" if necessary.
        /// </summary>
        public Visibility NoSinglesVisibility
        {
            get
            {
                return _noSinglesVisibility;
            }
            set
            {
                if (value != _noSinglesVisibility)
                {
                    _noSinglesVisibility = value;
                    NotifyPropertyChanged("NoSinglesVisibility");
                }
            }
        }

        private Visibility _noSimilarVisibility;
        /// <summary>
        /// NoSimilarVisibility property.
        /// This property is used in the UI to show "no similar artists available" if necessary.
        /// </summary>
        public Visibility NoSimilarVisibility
        {
            get
            {
                return _noSimilarVisibility;
            }
            set
            {
                if (value != _noSimilarVisibility)
                {
                    _noSimilarVisibility = value;
                    NotifyPropertyChanged("NoSimilarVisibility");
                }
            }
        }

        /// <summary>
        /// MainViewModel's IsDataLoaded property;
        /// </summary>
        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Loads local audio information and creates a model for local artists to be shown in Favourites view.
        /// </summary>
        public void LoadData()
        {
            mediaLib = new MediaLibrary();
            int totalTrackCount = 0;
            int totalArtistCount = 0;

            foreach (Artist a in mediaLib.Artists)
            {
                if (a.Songs.Count == 0) continue; // Unknown artist with 0 tracks encountered
                string artist = a.Name;
                int trackCount = a.Songs.Count;
                int playCount = 0;

                // check the play count of artist's tracks
                foreach (Song s in a.Songs)
                {
                    playCount += s.PlayCount;
                }

                // search correct index for artist based on artist's play count
                bool artistAdded = false;
                for (int i = 1; i < LocalAudio.Count; i++)
                {
                    if (Convert.ToInt16(LocalAudio[i].PlayCount) < playCount)
                    {
                        this.LocalAudio.Insert(i, new ArtistModel()
                        { 
                            Name = artist, 
                            LocalTrackCount = Convert.ToString(trackCount), 
                            PlayCount = Convert.ToString(playCount) 
                        });
                        artistAdded = true;
                        break;
                    }
                }
                if (artistAdded == false)
                {
                    this.LocalAudio.Add(new ArtistModel() 
                    { 
                        Name = artist, 
                        LocalTrackCount = Convert.ToString(trackCount), 
                        PlayCount = Convert.ToString(playCount) 
                    });
                }

                totalTrackCount += trackCount;
                totalArtistCount++;
            }

            // Continue with only the top 20 favourite artists
            int removeIndex = App.ViewModel.LocalAudio.Count - 1;
            while (removeIndex > 20)
            {
                App.ViewModel.LocalAudio.RemoveAt(removeIndex);
                removeIndex--;
            }

            foreach (ArtistModel m in App.ViewModel.LocalAudio)
            {
                // Divide local artists into two "size categories"
                if (m.Name == "MusicExplorerTitlePlaceholder") continue;
                int artistsWithMoreTracks = 0;
                int trackCount = Convert.ToInt16(m.LocalTrackCount);
                for (int i = 0; i < App.ViewModel.LocalAudio.Count; i++)
                {
                    if (Convert.ToInt16(App.ViewModel.LocalAudio[i].LocalTrackCount) > trackCount)
                        artistsWithMoreTracks++;
                }
                double artistRelation = (double)artistsWithMoreTracks / (double)totalArtistCount;

                if (artistRelation < 0.5)
                {
                    m.ItemHeight = "200";
                    m.ItemWidth = "206";
                }
                else
                {
                    m.ItemHeight = "100";
                    m.ItemWidth = "206";
                }
            }

            if (LocalAudio.Count <= 0)
            {
                NoFavouritesVisibility = Visibility.Visible;
            }
            else
            {
                NoFavouritesVisibility = Visibility.Collapsed;
            }

            this.IsDataLoaded = true;
        }

        /// <summary>
        /// Utility function to find out if an artist is present in device.
        /// </summary>
        /// <param name="artistName">Name of the artist.</param>
        /// <returns>true if artist exists in device, false if not.</returns>
        public bool IsLocalArtist(string artistName)
        {
            bool ret = false;
            artistName = artistName.ToLower();
            foreach (Artist a in mediaLib.Artists)
            {
                string comparedNameLower = a.Name.ToLower();
                if (comparedNameLower == artistName)
                {
                    ret = true;
                    break;
                }
            }
            return ret;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoThumbnailer
{


    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class VideoThumbnailerConfig
    {

        private VideoThumbnailerConfigFfmpeg ffmpegField;

        private VideoThumbnailerConfigSettings settingsField;

        private VideoThumbnailerConfigDir[] pathsField;

        /// <remarks/>
        public VideoThumbnailerConfigFfmpeg ffmpeg
        {
            get
            {
                return this.ffmpegField;
            }
            set
            {
                this.ffmpegField = value;
            }
        }

        /// <remarks/>
        public VideoThumbnailerConfigSettings settings
        {
            get
            {
                return this.settingsField;
            }
            set
            {
                this.settingsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("dir", IsNullable = false)]
        public VideoThumbnailerConfigDir[] paths
        {
            get
            {
                return this.pathsField;
            }
            set
            {
                this.pathsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class VideoThumbnailerConfigFfmpeg
    {

        private string ffmpeg_pathField;

        private string ffprobe_pathField;

        /// <remarks/>
        public string ffmpeg_path
        {
            get
            {
                return this.ffmpeg_pathField;
            }
            set
            {
                this.ffmpeg_pathField = value;
            }
        }

        /// <remarks/>
        public string ffprobe_path
        {
            get
            {
                return this.ffprobe_pathField;
            }
            set
            {
                this.ffprobe_pathField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class VideoThumbnailerConfigSettings
    {

        private byte thumbcountField;

        private byte thumbqualityField;

        private bool info_json_generationField;

        private string subdirField;

        private bool remove_old_firstField;

        private byte first_thumbnail_secField;

        private byte last_thumbnail_secField;

        private bool resursiveField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte thumbcount
        {
            get
            {
                return this.thumbcountField;
            }
            set
            {
                this.thumbcountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte thumbquality
        {
            get
            {
                return this.thumbqualityField;
            }
            set
            {
                this.thumbqualityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool info_json_generation
        {
            get
            {
                return this.info_json_generationField;
            }
            set
            {
                this.info_json_generationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string subdir
        {
            get
            {
                return this.subdirField;
            }
            set
            {
                this.subdirField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool remove_old_first
        {
            get
            {
                return this.remove_old_firstField;
            }
            set
            {
                this.remove_old_firstField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte first_thumbnail_sec
        {
            get
            {
                return this.first_thumbnail_secField;
            }
            set
            {
                this.first_thumbnail_secField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte last_thumbnail_sec
        {
            get
            {
                return this.last_thumbnail_secField;
            }
            set
            {
                this.last_thumbnail_secField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool resursive
        {
            get
            {
                return this.resursiveField;
            }
            set
            {
                this.resursiveField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class VideoThumbnailerConfigDir
    {

        private bool remove_old_firstField;

        private bool remove_old_firstFieldSpecified;

        private bool resursiveField;

        private bool resursiveFieldSpecified;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool remove_old_first
        {
            get
            {
                return this.remove_old_firstField;
            }
            set
            {
                this.remove_old_firstField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool remove_old_firstSpecified
        {
            get
            {
                return this.remove_old_firstFieldSpecified;
            }
            set
            {
                this.remove_old_firstFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool resursive
        {
            get
            {
                return this.resursiveField;
            }
            set
            {
                this.resursiveField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool resursiveSpecified
        {
            get
            {
                return this.resursiveFieldSpecified;
            }
            set
            {
                this.resursiveFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }


}

namespace DataAnalysisSystem.DTO.DatasetDTO
{
    public class MapDatasetToObjectViewModel
    {
        public string DatasetName { get; set; }

        public byte[] DatasetContentByteArray { get; set; }
        public string DatasetContentString { get; set; }

        public string InputFileName { get; set; }
        public string InputFileFormat { get; set; }
    }
}

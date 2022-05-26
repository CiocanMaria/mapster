﻿using Mapster.Common.MemoryMappedTypes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Mapster.Rendering
{

    public static class TileRenderer
    {
        public static BaseShape Tessellate(this MapFeatureData feature, ref BoundingBox boundingBox, ref PriorityQueue<BaseShape, int> shapes)
        {
            BaseShape? baseShape = null;
            baseShape = new Road(feature.Coordinates);

            var featureType = feature.Type;
            //if (feature.Properties.Any(p => p.Key == "highway" && MapFeature.HighwayTypes.Any(v => p.Value.StartsWith(v))))
            if (feature.Properties.Any(p => p == MapFeatureData.types.highway))
            {
                var coordinates = feature.Coordinates;
                var road = new Road(coordinates);
                baseShape = road;
                shapes.Enqueue(road, road.ZIndex);
            }
            //else if (feature.Properties.Any(p => p.Key.StartsWith("water")) && feature.Type != GeometryType.Point)
            else if (feature.Properties.Any(p => p == MapFeatureData.types.water))
            {
                var coordinates = feature.Coordinates;
                /*
                                var waterway = new Waterway(coordinates, feature.Type == GeometryType.Polygon);
                                baseShape = waterway;
                                shapes.Enqueue(waterway, waterway.ZIndex);
                            }
                            else if (feature.Properties.Any(p => p == MapFeatureData.types.natural))
                            {
                                var coordinates = feature.Coordinates;

                                var waterway = new Waterway(coordinates, feature.Type == GeometryType.Polygon);
                                baseShape = waterway;
                                shapes.Enqueue(waterway, waterway.ZIndex);
                            }
                            else if (feature.Properties.Any(p => p == MapFeatureData.types.railway))
                            {
                                var coordinates = feature.Coordinates;


                                */
                var waterway = new Waterway(coordinates, feature.Type == GeometryType.Polygon);
                baseShape = waterway;
                shapes.Enqueue(waterway, waterway.ZIndex);
            }
            else if (Border.ShouldBeBorder(feature))
            {
                var coordinates = feature.Coordinates;
                var border = new Border(coordinates);
                baseShape = border;
                shapes.Enqueue(border, border.ZIndex);
            }
            else if (PopulatedPlace.ShouldBePopulatedPlace(feature))
            {
                var coordinates = feature.Coordinates;
                var popPlace = new PopulatedPlace(coordinates, feature);
                baseShape = popPlace;
                shapes.Enqueue(popPlace, popPlace.ZIndex);
            }
            //else if (feature.Properties.Any(p => p.Key.StartsWith("railway")))

            else if (feature.Properties.Any(p => p == MapFeatureData.types.railway))
            {
                var coordinates = feature.Coordinates;
                var railway = new Railway(coordinates);
                baseShape = railway;
                shapes.Enqueue(railway, railway.ZIndex);
            }
            //else if (feature.Properties.Any(p => p.Key.StartsWith("natural") && featureType == GeometryType.Polygon))
            else if (feature.Properties.Any(p => p == MapFeatureData.types.natural))
            {
                var coordinates = feature.Coordinates;
                var geoFeature = new GeoFeature(coordinates, feature);
                baseShape = geoFeature;
                shapes.Enqueue(geoFeature, geoFeature.ZIndex);
            }
            //else if (feature.Properties.Any(p => p.Key.StartsWith("boundary") && p.Value.StartsWith("forest")))
            else if (feature.Properties.Any(p => p == MapFeatureData.types.boundary))
            {
                var coordinates = feature.Coordinates;
                var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Forest);
                baseShape = geoFeature;
                shapes.Enqueue(geoFeature, geoFeature.ZIndex);
            }
            //else if (feature.Properties.Any(p => p.Key.StartsWith("landuse") && (p.Value.StartsWith("forest") || p.Value.StartsWith("orchard"))))

            else if (feature.Properties.Any(p => p == MapFeatureData.types.landuse))
            {
                var coordinates = feature.Coordinates;
                var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Forest);
                baseShape = geoFeature;
                shapes.Enqueue(geoFeature, geoFeature.ZIndex);
            }
            // else if (feature.Type == GeometryType.Polygon && feature.Properties.Any(p
            //              => p.Key.StartsWith("landuse") && (p.Value.StartsWith("residential") || p.Value.StartsWith("cemetery") || p.Value.StartsWith("industrial") || p.Value.StartsWith("commercial") ||
            //                                                 p.Value.StartsWith("square") || p.Value.StartsWith("construction") || p.Value.StartsWith("military") || p.Value.StartsWith("quarry") ||
            //                                                p.Value.StartsWith("brownfield"))))
            else if (feature.Type == GeometryType.Polygon && feature.Properties.Any(p => p
                  == MapFeatureData.types.railway))
            {
                var coordinates = feature.Coordinates;
                var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Residential);
                baseShape = geoFeature;
                shapes.Enqueue(geoFeature, geoFeature.ZIndex);
            }
            //else if (feature.Type == GeometryType.Polygon && feature.Properties.Any(p
            //            => p.Key.StartsWith("landuse") && (p.Value.StartsWith("farm") || p.Value.StartsWith("meadow") || p.Value.StartsWith("grass") || p.Value.StartsWith("greenfield") ||
            //                                              p.Value.StartsWith("recreation_ground") || p.Value.StartsWith("winter_sports") || p.Value.StartsWith("allotments"))))
            else if (feature.Type == GeometryType.Polygon && feature.Properties.Any(p => p == MapFeatureData.types.admin_level))
            {
                var coordinates = feature.Coordinates;
                var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Plain);
                baseShape = geoFeature;
                shapes.Enqueue(geoFeature, geoFeature.ZIndex);
            }
            // else if (feature.Type == GeometryType.Polygon &&
            // feature.Properties.Any(p => p.Key.StartsWith("landuse") && (p.Value.StartsWith("reservoir") || p.Value.StartsWith("basin"))))
            else if (feature.Type == GeometryType.Polygon && feature.Properties.Any(p => p == MapFeatureData.types.landuse))
            {
                var coordinates = feature.Coordinates;
                var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Water);
                baseShape = geoFeature;
                shapes.Enqueue(geoFeature, geoFeature.ZIndex);
            }
            //else if (feature.Type == GeometryType.Polygon && feature.Properties.Any(p => p.Key.StartsWith("building")))

            else if (feature.Type == GeometryType.Polygon && feature.Properties.Any(p => p == MapFeatureData.types.building))
            {
                var coordinates = feature.Coordinates;
                var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Residential);
                baseShape = geoFeature;
                shapes.Enqueue(geoFeature, geoFeature.ZIndex);
            }
            //else if (feature.Type == GeometryType.Polygon && feature.Properties.Any(p => p.Key.StartsWith("leisure")))

            else if (feature.Type == GeometryType.Polygon && feature.Properties.Any(p => p == MapFeatureData.types.leisure))
            {
                var coordinates = feature.Coordinates;
                var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Residential);
                baseShape = geoFeature;
                shapes.Enqueue(geoFeature, geoFeature.ZIndex);
            }
            //else if (feature.Type == GeometryType.Polygon && feature.Properties.Any(p => p.Key.StartsWith("amenity")))

            else if (feature.Type == GeometryType.Polygon && feature.Properties.Any(p => p == MapFeatureData.types.amenity))
            {
                var coordinates = feature.Coordinates;
                var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Residential);
                baseShape = geoFeature;
                shapes.Enqueue(geoFeature, geoFeature.ZIndex);
            }

            // for (var j = 0; j < baseShape.ScreenCoordinates.Length; ++j)
            //{
            /* boundingBox.MinX = Math.Min(boundingBox.MinX, baseShape.ScreenCoordinates[j].X);
             boundingBox.MaxX = Math.Max(boundingBox.MaxX, baseShape.ScreenCoordinates[j].X);
             boundingBox.MinY = Math.Min(boundingBox.MinY, baseShape.ScreenCoordinates[j].Y);
             boundingBox.MaxY = Math.Max(boundingBox.MaxY, baseShape.ScreenCoordinates[j].Y);*/
            if (baseShape != null)
            {
                for (var j = 0; j < baseShape.ScreenCoordinates.Length; ++j)
                {
                    boundingBox.MinX = Math.Min(boundingBox.MinX, baseShape.ScreenCoordinates[j].X);
                    boundingBox.MaxX = Math.Max(boundingBox.MaxX, baseShape.ScreenCoordinates[j].X);
                    boundingBox.MinY = Math.Min(boundingBox.MinY, baseShape.ScreenCoordinates[j].Y);
                    boundingBox.MaxY = Math.Max(boundingBox.MaxY, baseShape.ScreenCoordinates[j].Y);
                }
            }


            return baseShape;
        }
        public static Image<Rgba32> Render(this PriorityQueue<BaseShape, int> shapes, BoundingBox boundingBox, int width, int height)
        {
            var canvas = new Image<Rgba32>(width, height);

            // Calculate the scale for each pixel, essentially applying a normalization
            var scaleX = canvas.Width / (boundingBox.MaxX - boundingBox.MinX);
            var scaleY = canvas.Height / (boundingBox.MaxY - boundingBox.MinY);
            var scale = Math.Min(scaleX, scaleY);

            // Background Fill
            canvas.Mutate(x => x.Fill(Color.White));
            while (shapes.Count > 0)
            {
                var entry = shapes.Dequeue();
                // FIXME: Hack
                if (entry.ScreenCoordinates.Length < 2)
                {
                    continue;
                }
                entry.TranslateAndScale(boundingBox.MinX, boundingBox.MinY, scale, canvas.Height);
                canvas.Mutate(x => entry.Render(x));
            }

            return canvas;
        }

        public struct BoundingBox
        {
            public float MinX;
            public float MaxX;
            public float MinY;
            public float MaxY;
        }
    }
}
    

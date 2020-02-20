using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GeoJsonDemo
{
    public class ApplicationManager
    {
        public LocationRequest LocationRequest { get; private set; }

        public void UpdateLocation(TimeSpan time)
        {
            Task.Run(() =>
            {
                UpdateLocationRequest(time);
                Device.StartTimer(time, () =>
                {
                    UpdateLocationRequest(time);
                    return true;
                });
            });
        }

        async void UpdateLocationRequest(TimeSpan time)
        {
            LocationRequest = await GetLocationAsync();
            if (!LocationRequest.IsSuccessLocationCode)
            {
                Print($"{LocationRequest.Message}");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append($"Actualizando ubicación a las {DateTime.Now}\n");
            builder.Append($"Ubicación del usuario: {LocationRequest?.Location?.Latitude ?? 0.0}, {LocationRequest?.Location?.Longitude ?? 0.0}");
            Print(builder.ToString());
        }

        public async Task<LocationRequest> GetLocationAsync(double timeout = 5)
        {
            LocationRequest request;
            try
            {
                GeolocationRequest geolocationRequest = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(timeout));
                Location location = await Geolocation.GetLocationAsync(geolocationRequest);
                if (location != null)
                {
                    request = LocationRequest.CreateLocationRequest("Localización obtenida", "Localización obtenida", LocationRequest.LocationCode.Success, location);
                }
                else
                {
                    request = LocationRequest.CreateLocationRequest("Localización no disponible", "La localización del dispositivo es nula", LocationRequest.LocationCode.NullRequest);
                }
                return request;
            }
            catch (FeatureNotSupportedException ex)
            {
                request = LocationRequest.CreateLocationRequest("Ubicación no disponible, el dispositivo no soporta la geocodificación", $"{ex.StackTrace}", LocationRequest.LocationCode.NotSupported);
                return request;
            }
            catch (FeatureNotEnabledException ex)
            {
                request = LocationRequest.CreatePlacemarkRequest("Ubicación no disponible, por favor, habilita la geocodificación del dispositivo", $"{ex.StackTrace}", LocationRequest.LocationCode.NotEnabled);
                return request;
            }
            catch (PermissionException ex)
            {
                request = LocationRequest.CreatePlacemarkRequest("Ubicación no disponible, por favor, asigna los permisos necesarios a la aplicación para obtener la ubicación", $"{ex.StackTrace}", LocationRequest.LocationCode.NotPermission);
                return request;
            }
            catch (Exception ex)
            {
                request = LocationRequest.CreatePlacemarkRequest("Ubicación no disponible", $"{ex.StackTrace}", LocationRequest.LocationCode.Unknown);
                return request;
            }
        }

        public async Task<LocationRequest> GetPlacemarkAsync(Location location)
        {
            if (location == null)
            {
                return LocationRequest.CreatePlacemarkRequest("No fue posible obtener la marca de posición del dispositivo", "La ubicación del dispositivo es nula/vacía", LocationRequest.LocationCode.Unknown);
            }
            return await GetPlacemarkAsync(location.Latitude, location.Longitude);
        }

        public async Task<LocationRequest> GetPlacemarkAsync(double latitude, double longitude)
        {
            LocationRequest request;
            try
            {
                IEnumerable<Placemark> placemarks = await Geocoding.GetPlacemarksAsync(latitude, longitude);
                if (placemarks != null)
                {
                    Placemark placemark = placemarks.FirstOrDefault();
                    if (placemark != null)
                    {
                        request = LocationRequest.CreatePlacemarkRequest("Posición obtenida", "Posición obtenida", LocationRequest.LocationCode.Success, placemark);
                    }
                    else
                    {
                        request = LocationRequest.CreatePlacemarkRequest("Posición no disponible", "No fue posible obtener la geocodificación del dispositivo", LocationRequest.LocationCode.NullRequest);
                    }
                }
                else
                {
                    request = LocationRequest.CreatePlacemarkRequest("Geocodificación del dispositivo no disponible", "El objeto con nombre 'placemarks' es nulo o está vacío", LocationRequest.LocationCode.NullRequest);
                }
                return request;
            }
            catch (FeatureNotEnabledException ex)
            {
                request = LocationRequest.CreatePlacemarkRequest("Posición no disponible, por favor, habilita la geocodificación del dispositivo", $"{ex.StackTrace}", LocationRequest.LocationCode.NotEnabled);
                return request;
            }
            catch (FeatureNotSupportedException ex)
            {
                request = LocationRequest.CreatePlacemarkRequest("Posición no disponible, el dispositivo no soporta la geocodificación", $"{ex.StackTrace}", LocationRequest.LocationCode.NotSupported);
                return request;
            }
            catch (PermissionException ex)
            {
                request = LocationRequest.CreatePlacemarkRequest("Posición no disponible, por favor, asigna los permisos necesarios a la aplicación para obtener la ubicación", $"{ex.StackTrace}", LocationRequest.LocationCode.NotPermission);
                return request;
            }
            catch (Exception ex)
            {
                request = LocationRequest.CreatePlacemarkRequest("Posición no disponible", $"{ex.StackTrace}", LocationRequest.LocationCode.Unknown);
                return request;
            }
        }

        public Task<string> ReadResource(string resourceName)
        {
            Assembly assembly = IntrospectionExtensions.GetTypeInfo(typeof(ApplicationManager)).Assembly;
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                return Task.FromResult("");
            }
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEndAsync();
        }

        static ApplicationManager current;
        protected ApplicationManager() { }
        public static ApplicationManager Current
        {
            get
            {
                if (current == null)
                {
                    current = new ApplicationManager();
                }
                return current;
            }
        }

        public static void Print(string message, string fromClass = null, string fromMethod = null)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(!string.IsNullOrEmpty(fromClass) && !string.IsNullOrWhiteSpace(fromClass) ? $"[c-{fromClass}] - " : "");
            builder.Append(!string.IsNullOrEmpty(fromMethod) && !string.IsNullOrWhiteSpace(fromMethod) ? $"[m-{fromMethod}] - " : "");
            builder.Append(!string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message) ? message : "");
            System.Diagnostics.Debug.WriteLine($"{builder.ToString()}");
        }
    }

    public class LocationRequest
    {
        public bool IsSuccessLocationCode { get; set; }
        public Location Location { get; set; }
        public Placemark Placemark { get; set; }
        public string Address { get; set; }
        public string Message { get; set; }
        public string Reason { get; set; }
        public LocationCode Code { get; set; }

        public LocationRequest()
        {
            IsSuccessLocationCode = false;
            Location = null;
            Placemark = null;
            Address = "";
            Message = "";
            Reason = "";
            Code = LocationCode.Initialize;
        }

        public static LocationRequest CreateLocationRequest(string message, string reason, LocationCode code, Location location = null)
        {
            LocationRequest request = new LocationRequest();
            request.Message = message;
            request.Reason = reason;
            request.Code = code;
            request.Location = location;
            request.IsSuccessLocationCode = code == LocationCode.Success && location != null ? true : false;
            return request;
        }

        public static LocationRequest CreatePlacemarkRequest(string message, string reason, LocationCode code, Placemark placemark = null)
        {
            LocationRequest request = new LocationRequest();
            request.Message = message;
            request.Reason = reason;
            request.Code = code;
            request.Placemark = placemark;
            request.IsSuccessLocationCode = code == LocationCode.Success && placemark != null ? true : false;
            if (placemark != null)
            {
                string address = $"{placemark.Thoroughfare}, {placemark.SubThoroughfare}, {placemark.SubLocality}, {placemark.PostalCode}, {placemark.Locality}, {placemark.AdminArea}, {placemark.CountryName}";
                if (address == ", , , , , , ")
                {
                    address = $"Ubicación no identificada";
                }
                request.Address = address;
            }
            return request;
        }

        public enum LocationCode
        {
            Initialize,
            Success,
            NotSupported,
            NotEnabled,
            NotPermission,
            NullRequest,
            Unknown
        }
    }
}

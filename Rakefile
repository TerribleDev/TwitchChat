require 'json'
require 'rake/clean'

CLEAN.include(['**/bin', '**/obj'])

desc 'clean, build, publish'
task :preflight => [:clean, :build, :publish]

desc 'restore nuget packages things'
task :restore do
    sh "dotnet restore src/TwitchChat"
end

desc 'build our code'
task :build do
    sh "dotnet build src/TwitchChat -c release"
end

desc 'publish all runtimes'
task :publish => :restore do
    text = File.read('src/TwitchChat/project.json')
    JSON.parse(text)["runtimes"].each{|t, v| 
    sh "dotnet publish src/TwitchChat/ -c Release -r #{t}"
}
end
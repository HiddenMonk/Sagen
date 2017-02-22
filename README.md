# Sagen

**Sagen** _(German for "to say")_ is my attempt at making a text-to-speech engine aimed at .NET developers who don't have thousands of dollars at their disposal to license a commercial speech synthesis solution. In many ways, it is an experiment and continual learning experience for me, as I am in no way well-versed in speech science, phonetics, or vocal acoustics; I simply want to see how far I can go with original research and lots of patience.

## Rationale

Some might ask me why I'd bother. After all, there are tons of TTS engines out there already. In short, I don't feel like people have enough options.

Aside from often being prohibitively expensive, commercial TTS technologies are frequently restrictive in their available customization options for voices, voice parameters, and context-sensitive vocal qualities like intonation, stress, and timbre which are necessary to convey emotion and meaning. Concatenative synthesizers and other similar "realistic" TTS technologies based on databases of recorded samples also are relatively slow, leave a large memory footprint, and require each voice to be installed separately. There are also many free options for speech synthesizers, but they often have sparse, confusing, or convoluted documentation, or are locked down to one specific language (e.g. Java). While all TTS libraries have advantages and disadvantages, I feel like the .NET crowd would welcome a TTS solution specifically made for them.

My goal with this project is not _necessarily_ to produce "something better", but to instead offer a user-friendly TTS engine with a respectable amount of configurability, flexibility, and performance. The best part? **It's free.**

## What's planned

Here is a short list of major features that will be supported:
* Text-to-speech based on [formant synthesis](https://en.wikipedia.org/wiki/Speech_synthesis#Formant_synthesis) and physically-based vocal filtering
* Plentiful parameters for tuning how voices sound (age, sex, vocal force, hoarseness, etc...)
* Support for direct playback, WAV exporting, and sending audio data via `System.Stream`
* Multiple options for sample format and rate (export only)
* Support for [X-SAMPA](https://en.wikipedia.org/wiki/X-SAMPA)-based pronunciation lexicons
* Multiple language support (English and German are currently prioritized)
* [Heteronym](https://en.wikipedia.org/wiki/Heteronym_(linguistics)) resolution
* Singing?!

It is currently a heavy work-in-progress, and I welcome your input and/or contributions.

## Licensing

This project is made available under the MIT License and is completely free for anyone to use, for any purpose, without the burdens of licensing costs or royalties.
